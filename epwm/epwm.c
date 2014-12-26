//punkt 15.2.3.3 w dokumentacji
//Table 15-43

#include <stdlib.h>
#include <stdio.h>
#include <hw/inout.h>
#include <sys/mman.h>
#include <sys/neutrino.h>
#include <stdint.h>

#define EPWM			0x48300200
#define TBPRD			0xA
#define TBPHS			0x6
#define TBCTL 			0x0
#define CMPCTL			0xE
#define AQCTLA			0x16
#define CMPA 			0x12

#define TB_UP 			0x1
#define TB_DISABLE 		0x0
#define TB_SHADOW		0x0
#define TB_SYNC_DISABLE 0x3
#define CC_SHADOW		0x0
#define CC_CTR_ZERO 	0x0
#define AQ_CLEAR 		0x1
#define AQ_SET 			0x2


#define AM335X_GPIO_SIZE                0x00001000
#define AM335X_GPIO1_BASE               0x4804C000
#define GPIO_DATAOUT                    0x13C
#define GPIO_OE                         0x134

int main(int argc, char *argv[]) {
	uintptr_t       pwm_base, gpio_base;
	int val;

	ThreadCtl (_NTO_TCTL_IO,NULL);

	pwm_base = mmap_device_io(0x10000000, EPWM);
	if(pwm_base == MAP_DEVICE_FAILED){
		perror("Can't map device I/O");
		return 0;
	}

	gpio_base = mmap_device_io(AM335X_GPIO_SIZE, AM335X_GPIO1_BASE);

	if(gpio_base == MAP_DEVICE_FAILED){
		perror("Can't map device I/O");
		return 0;
	}

	out16(pwm_base+TBPRD, 1200);
	out16(pwm_base+TBPHS, 0);
	val = TB_UP | TB_DISABLE << 2 | TB_SHADOW << 3 | TB_SYNC_DISABLE << 4;
	out16(pwm_base+TBCTL, val);
	out8(pwm_base+CMPCTL, 0);
	val = AQ_CLEAR << 2 | AQ_SET << 4;
	out8(pwm_base+AQCTLA, val);
	out16(pwm_base+CMPA, 700);
	int change = 1;
	while(1){
		if (change == 1){
			out16(pwm_base+CMPA, 200);
			change = 0;
			val = 1<<21;
		}
		else {
			out16(pwm_base+CMPA, 700);
			change = 1;
			val = 1<<22;
		}
		out32(gpio_base+GPIO_DATAOUT, val);
		sleep(1);
	}
	munmap_device_io(pwm_base, 0x10000000);
	munmap_device_io(gpio_base, AM335X_GPIO_SIZE);
	return EXIT_SUCCESS;
}
