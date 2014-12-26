#include <stdlib.h>
#include <stdio.h>
#include <hw/inout.h>
#include <sys/mman.h>
#include <sys/neutrino.h>
#include <stdint.h>

#define AM335X_GPIO_SIZE                0x00100000
#define AM335X_GPIO1_BASE               0x4804C000
#define GPIO_DATAOUT                    0x13C
#define GPIO_OE                         0x134
#define GPIO_DATAIN						0x138
#define GPIO_CLEARDATAOUT				0x190

#define LED0                (1<<21)
#define LED1                (1<<22)
#define LED2                (1<<23)
#define LED3                (1<<24)
#define LEDS(n)             ((n&0xf)<<21)

#define OUTPUT		0
#define INPUT 		1

int pinMode(int pin, int io, uintptr_t *gpio_base);
int dgWrite(int pin, int val, uintptr_t *gpio_base);
int dgRead(int pin, uintptr_t *gpio_base);
int resetAll();
int main(int argc, char *argv[])
{
	int i = 0;
	int n = 0;
	uintptr_t       gpio_base;
	uint32_t        val;
	int 			val2;
	ThreadCtl (_NTO_TCTL_IO,NULL);

    // Map device registers

	gpio_base = mmap_device_io(AM335X_GPIO_SIZE, AM335X_GPIO1_BASE);
    if(gpio_base == MAP_DEVICE_FAILED){
        perror("Can't map device I/O");
        return 0;
    }

    pinMode(4, OUTPUT, &gpio_base);
    pinMode(5, INPUT, &gpio_base);
    resetAll(&gpio_base);
    int a = 0;

    val = dgRead(5, &gpio_base);
    while (!val){
    	val = dgRead(5, &gpio_base);
    	dgWrite(4, 1, &gpio_base);
    }



    val = LED0 | LED2;
    out32(gpio_base + GPIO_DATAOUT, val);
    munmap_device_io(gpio_base, AM335X_GPIO_SIZE);

    return 1;
}
int pinMode(int pin, int io, uintptr_t *gpio_base){
	int val = io << pin;
	out32(*gpio_base + GPIO_OE, val);
	return 0;
}
int dgWrite(int pin, int val, uintptr_t *gpio_base){
	int value;

	int read = in32(*gpio_base + GPIO_DATAOUT);
	if (val == 1)
		value  = read | (1 << pin);
	else
		value = read & ~(1 << pin);
	out32(*gpio_base + GPIO_DATAOUT, value);
	return 0;
}
int dgRead(int pin, uintptr_t *gpio_base){
	int val = 1 << pin;
	int read = in32(*gpio_base + GPIO_DATAIN);
	return !(val & read);
}
int resetAll(uintptr_t *gpio_base){
	int value = 0;
	out32(*gpio_base + GPIO_DATAOUT, value);
	return 0;
}
