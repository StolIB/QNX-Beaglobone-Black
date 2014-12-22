#include <stdlib.h>
#include <stdio.h>
#include <pthread.h>
#include <hw/inout.h>
#include <sys/mman.h>
#include <sys/neutrino.h>
#include <sched.h>
#include <math.h>
#include <unistd.h>


pthread_t threads[3];
pthread_mutex_t mutex;
unsigned int j = 0;


void *foo(void *argv){
	double a = 0;
	long int b = 0;
	int mut = (int)argv;
	if (mut)
		pthread_mutex_lock(&mutex);
	for (b = 0; b < 1000000; b++){
		j++;
		a = (b*j/0.234*j*4.28342342)*(b+0.34234*b);
	}
	if (mut)
		pthread_mutex_unlock(&mutex);

	printf("tid:%d, %d\n", pthread_self(), j);


	pthread_exit((void *) 0);

	return NULL;
}

int main(int argc,char *argv[])
  {
	 int status;

     pthread_attr_t attr;
     pthread_attr_init( &attr );
     pthread_attr_setdetachstate(&attr, PTHREAD_CREATE_JOINABLE);
     pthread_mutex_init(&mutex, NULL);

     int i;
     //bez mutexa
     for (i = 0; i < 3; i++)
    	 pthread_create(&threads[i], &attr, foo, 0);
     for (i = 0; i < 3; i++)
     pthread_join(threads[i], (void **)&status);

     printf("main j:%d\n\n", j);

     j = 0;

     //z mutexem
     for (i = 0; i < 3; i++)
		 pthread_create(&threads[i], &attr, foo, 1);
	  for (i = 0; i < 3; i++)
		  pthread_join(threads[i], (void **)&status);

     printf("main j:%d", j);
     pthread_mutex_destroy(&mutex);
     pthread_attr_destroy(&attr);
     //pthread_exit(NULL);


     return 0;
  }
