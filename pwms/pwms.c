#include <stdlib.h>
#include <stdio.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <sys/types.h>
#include <pthread.h>
#include <sched.h>
#include <time.h>

#include <hw/inout.h>
#include <sys/mman.h>
#include <sys/neutrino.h>

#define AM335X_GPIO_SIZE                0x00100000
#define AM335X_GPIO1_BASE               0x4804C000
#define GPIO_DATAOUT                    0x13C
#define GPIO_OE                         0x134
#define GPIO_DATAIN						0x138
#define GPIO_CLEARDATAOUT				0x190

int sockfd,n;
struct sockaddr_in servaddr,cliaddr;
socklen_t len;
char mesg[1000];
pthread_t communication, pwm;
int threads_num = 10;
pthread_t primes[10];
uintptr_t gpio_base;

struct timespec start, stop, start2, stop2, start3, stop3;

int frequency = 1;
void *do_primes()
{
	unsigned long i, num, _primes = 0;
    double t2;
    while(1){
    	num++;
        for (i = 2; (i <= num) && (num % i != 0); ++i);
        if (i == num){
            ++_primes;
            //printf("tid:%d, prime: %d\n",pthread_self(), num);
        }
    }
    printf("Calculated %d primes.\n", _primes);
}
void *_pwm(){

	int i = 0;
	int state = 1;
	double t, t2;
	clock_gettime(CLOCK_REALTIME, &start);
	while(1){
		clock_gettime(CLOCK_REALTIME, &stop);
		t = (stop.tv_sec - start.tv_sec)*1000000000 + (stop.tv_nsec-start.tv_nsec);
		if (t >= (double)(1000000000/(frequency*2))){
			clock_gettime(CLOCK_REALTIME, &start);
			if (state == 1)
				out32(gpio_base + GPIO_DATAOUT, (1<<4) | (1<<21) | (0<<22));
			else
				out32(gpio_base + GPIO_DATAOUT, (0<<4) | (0<<21) | (1<<22));
			state = (++state)%2;
		}
	}
}
void *rec(){
	int rec;
	while(1)
	   {
		  len = sizeof(cliaddr);
	      n = recvfrom(sockfd,&rec,1000,0,(struct sockaddr *)&cliaddr,&len);
	      //sendto(sockfd,mesg,n,0,(struct sockaddr *)&cliaddr,sizeof(cliaddr));
	      //printf("-------------------------------------------------------\n");
	      //mesg[n] = 0;
	      if (ntohl(rec) > 0)
	    	  frequency = ntohl(rec);
	      else
	    	  frequency = 1;
	      printf("frequency: %d\n", frequency);
	      //printf("1/f: %fl\n", (double)(1000000000/frequency));
	   }
}
int main(int argc, char**argv)
{

	int p = 0;

	ThreadCtl (_NTO_TCTL_IO,NULL);
	gpio_base = mmap_device_io(AM335X_GPIO_SIZE, AM335X_GPIO1_BASE);
	if(gpio_base == MAP_DEVICE_FAILED){
		perror("Can't map device I/O");
		return 0;
	}
	out32(gpio_base + GPIO_OE, 0);

	struct sched_param params;

	pthread_attr_t attr;
	pthread_attr_init( &attr );
	//pthread_attr_setinheritsched(&attr, PTHREAD_EXPLICIT_SCHED);
	//params.sched_priority = 50;
	//pthread_attr_setschedparam(&attr, &params);

	pthread_attr_setdetachstate(&attr, PTHREAD_CREATE_JOINABLE);


	sockfd=socket(AF_INET,SOCK_DGRAM,0);
	bzero(&servaddr,sizeof(servaddr));
	servaddr.sin_family = AF_INET;
	servaddr.sin_addr.s_addr=htonl(INADDR_ANY);
	servaddr.sin_port=htons(5002);
	bind(sockfd,(struct sockaddr *)&servaddr,sizeof(servaddr));

	pthread_create(&communication, &attr, rec, NULL);
	//ThreadCreate(0,_pwm, NULL, &attr);
	pthread_create(&pwm, &attr, _pwm, NULL);
	printf("pwm: %d\n", pwm);
	pthread_setschedprio(pthread_self(), 30);

	int i = 0;
	if (0){
		for (i = 0; i < 1; i++)
			pthread_create(&primes[i], &attr, do_primes, NULL);
	}
	pthread_join(communication, NULL);

	munmap_device_io(gpio_base, AM335X_GPIO_SIZE);

}
