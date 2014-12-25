//192.168.1.103
#include <stdio.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>

int i = 12;
char ci[10];

float T = 10;
int k = 1;
float y = 0;
float y1 = 0;
int x = 0;
int _x = 0;
char _y[10];

void doprocessing (int sock)
{

    int n;

    char buffer[256];

    bzero(buffer,256);

    //n = read(sock,buffer,255);
    n = read(sock, &_x, 4, 0);
    x = ntohl(_x);
    //printf("x: %d\n", _x);
    if (n < 0)
    {
        perror("ERROR reading from socket");
        exit(1);
    }
    //printf("Message: %s\n",buffer);

    y = (1/T)*(k*x-y1)+y1;
    y1 = y;
    printf("y:%f x:%d\n", y, x);
    i++;
    itoa(i, ci, 10);
    itoa((int)(y*100), _y, 10);
    n = send(sock, &_y, 10, 0);
    //n = write(sock, (char *)i ,sizeof((char *)i));



    if (n < 0)
    {
        perror("ERROR writing to socket");
        exit(1);
    }
}

int main( int argc, char *argv[] )
{
    int sockfd, newsockfd, portno, clilen;
    char buffer[256];
    struct sockaddr_in serv_addr, cli_addr;
    int  n, pid;

    /* First call to socket() function */
    sockfd = socket(AF_INET, SOCK_STREAM, 0);
    if (sockfd < 0)
    {
        perror("ERROR opening socket");
        exit(1);
    }
    /* Initialize socket structure */
    bzero((char *) &serv_addr, sizeof(serv_addr));
    portno = 5001;
    serv_addr.sin_family = AF_INET;
    serv_addr.sin_addr.s_addr = INADDR_ANY;
    serv_addr.sin_port = htons(portno);

    /* Now bind the host address using bind() call.*/
    if (bind(sockfd, (struct sockaddr *) &serv_addr,
                          sizeof(serv_addr)) < 0)
    {
         perror("ERROR on binding");
         exit(1);
    }
    /* Now start listening for the clients, here
     * process will go in sleep mode and will wait
     * for the incoming connection
     */
    listen(sockfd,5);
    clilen = sizeof(cli_addr);
    while (1)
    {
        newsockfd = accept(sockfd,
                (struct sockaddr *) &cli_addr, &clilen);
        if (newsockfd < 0)
        {
            perror("ERROR on accept");
            exit(1);
        }
        /* Create child process */
        while(1)
            	doprocessing(newsockfd);
            exit(0);

    } /* end of while */
}
