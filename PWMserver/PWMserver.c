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
pthread_t primes[30];
uintptr_t gpio_base;

struct timespec start, stop, start2, stop2;

int frequency = 1;
void *do_primes()
{
    unsigned long i, num, primes = 0;
    //for (num = 1; num <= 10000000; ++num) {
    while(1){
    	num++;
        for (i = 2; (i <= num) && (num % i != 0); ++i);
        if (i == num){
            ++primes;
            printf("tid:%d, prime: %d\n",pthread_self(), num);
        }
    }
    printf("Calculated %d primes.\n", primes);
}
void *_pwm(){
	int state = 1;
	double t, t2;
	clock_gettime(CLOCK_REALTIME, &start);
	//clock_gettime(CLOCK_REALTIME, &start2);
	while(1){
		clock_gettime(CLOCK_REALTIME, &stop);
		t = (stop.tv_sec - start.tv_sec)*1000000000 + (stop.tv_nsec-start.tv_nsec);
		if (t >= (double)(1000000000/frequency)){

			clock_gettime(CLOCK_REALTIME, &start);
			//clock_gettime(CLOCK_REALTIME, &stop2);
			//t2 = (stop2.tv_sec - start2.tv_sec)*1000000000 + (stop2.tv_nsec-start2.tv_nsec);
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

	pthread_attr_t attr;
	pthread_attr_init( &attr );
	pthread_attr_setdetachstate(&attr, PTHREAD_CREATE_JOINABLE);

	sockfd=socket(AF_INET,SOCK_DGRAM,0);

	bzero(&servaddr,sizeof(servaddr));
	servaddr.sin_family = AF_INET;
	servaddr.sin_addr.s_addr=htonl(INADDR_ANY);
	servaddr.sin_port=htons(5002);
	bind(sockfd,(struct sockaddr *)&servaddr,sizeof(servaddr));

	pthread_create(&communication, &attr, rec, NULL);
	pthread_create(&pwm, &attr, _pwm, NULL);
	int i = 0;
	if (p){
		for (i = 0; i < 30; i++)
			pthread_create(&primes[i], &attr, do_primes, NULL);
	}
	pthread_join(communication, NULL);

	munmap_device_io(gpio_base, AM335X_GPIO_SIZE);

}
