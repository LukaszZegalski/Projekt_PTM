#include "stm32f4xx.h"

int a=0;
int b=0;
int c=0;
int d=0;
int e=5;

//zalaczenie lini
 void RCC_config(void)
{
	SystemInit();
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOA, ENABLE);
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOC, ENABLE);
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOD, ENABLE);
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM3, ENABLE);
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM2, ENABLE);
}
//timer zatrzymuj¹cy pojazd
void TIMER3_config(void)
{
	TIM_TimeBaseInitTypeDef TIM_TimeBaseStructure;
	/* Time base configuration */
	TIM_TimeBaseStructure.TIM_Period =9999; //9999;
	TIM_TimeBaseStructure.TIM_Prescaler = 79; //49;
	TIM_TimeBaseStructure.TIM_ClockDivision = TIM_CKD_DIV1;
	TIM_TimeBaseStructure.TIM_CounterMode =  TIM_CounterMode_Up;
	TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
	TIM_Cmd(TIM3, ENABLE);
}
//timer do przebiegu pwm
void TIMER2_config(void)
{
	TIM_TimeBaseInitTypeDef TIM_TimeBaseStructure;
	/* Time base configuration */
	TIM_TimeBaseStructure.TIM_Period =9999;
	TIM_TimeBaseStructure.TIM_Prescaler = 79;
	TIM_TimeBaseStructure.TIM_ClockDivision = TIM_CKD_DIV1;
	TIM_TimeBaseStructure.TIM_CounterMode =  TIM_CounterMode_Up;
	TIM_TimeBaseInit(TIM2, &TIM_TimeBaseStructure);
	TIM_Cmd(TIM2, ENABLE);
}
//konfiguracja pwm
void PWM(void)
{
	TIM_OCInitTypeDef TIM_OCInitStructure;
	TIM_OCInitStructure.TIM_OCMode = TIM_OCMode_PWM1;
	TIM_OCInitStructure.TIM_OutputState = TIM_OutputState_Enable;
	TIM_OCInitStructure.TIM_Pulse = 0;
	TIM_OCInitStructure.TIM_OCPolarity = TIM_OCPolarity_High;
	TIM_OC1Init(TIM2, &TIM_OCInitStructure);
	TIM_OC1PreloadConfig(TIM2, TIM_OCPreload_Enable);
}
//piny dla przebiegu pwm
void GPIO_PWM_config(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_0 | GPIO_Pin_5;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF;
	GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure.GPIO_PuPd = GPIO_PuPd_UP;
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_Init(GPIOA, &GPIO_InitStructure);
	GPIO_PinAFConfig(GPIOA, GPIO_PinSource0, GPIO_AF_TIM2);
	GPIO_PinAFConfig(GPIOA, GPIO_PinSource5, GPIO_AF_TIM2);
}
//piny do sterowania silnikiem port A
void ENGINE_GPIO_config(void)
{
	GPIO_InitTypeDef  GPIO_InitStructure;
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_1| GPIO_Pin_4 | GPIO_Pin_7;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_OUT;
	GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_100MHz;
	GPIO_InitStructure.GPIO_PuPd = GPIO_PuPd_NOPULL;
	GPIO_Init(GPIOA, &GPIO_InitStructure);
}
//piny do sterowania silnikiem port C, poniewa¿ nie dzia³a³ pin 6A
void ENGINE_GPIO_config2(void)
{
	GPIO_InitTypeDef  GPIO_InitStructure;
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_5;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_OUT;
	GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_100MHz;
	GPIO_InitStructure.GPIO_PuPd = GPIO_PuPd_NOPULL;
	GPIO_Init(GPIOC, &GPIO_InitStructure);
}
//piny od klawiatury
void GPIO_IN_config(void)
{
	GPIO_InitTypeDef  GPIO_InitStructure;
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_0 | GPIO_Pin_1 | GPIO_Pin_2 | GPIO_Pin_3 | GPIO_Pin_4 | GPIO_Pin_5 | GPIO_Pin_6 | GPIO_Pin_7;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN;
	GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_100MHz;
	GPIO_InitStructure.GPIO_PuPd = GPIO_PuPd_UP;
	GPIO_Init(GPIOD, &GPIO_InitStructure);
}

//funkcje sterujace silnikami
/* pin 1A,4A -> B2, B1 || pin 5C,7A -> A2, A1*/
void przod()
{
	e=10;
	TIM2->CCR1 = 10000;
	GPIO_SetBits(GPIOA, GPIO_Pin_7);
	GPIO_SetBits(GPIOA, GPIO_Pin_1);
	GPIO_ResetBits(GPIOA, GPIO_Pin_4);
	GPIO_ResetBits(GPIOC, GPIO_Pin_5);
}

void tyl()
{
	e=10;
	TIM2->CCR1 = 10000;
	GPIO_SetBits(GPIOC, GPIO_Pin_5);
	GPIO_SetBits(GPIOA, GPIO_Pin_4);
	GPIO_ResetBits(GPIOA, GPIO_Pin_7 | GPIO_Pin_1);
}

void lewo()
{
	e=10;
	TIM2->CCR1 = 6000;
	GPIO_SetBits(GPIOC, GPIO_Pin_5);
	GPIO_SetBits(GPIOA, GPIO_Pin_1);
	GPIO_ResetBits(GPIOA, GPIO_Pin_4 | GPIO_Pin_7);
}

void prawo()
{
	e=10;
	TIM2->CCR1 = 6000;
	GPIO_SetBits(GPIOA, GPIO_Pin_7);
	GPIO_SetBits(GPIOA, GPIO_Pin_4);
	GPIO_ResetBits(GPIOA, GPIO_Pin_1 );
	GPIO_ResetBits(GPIOC, GPIO_Pin_5 );
}

void stop(){
	GPIO_ResetBits(GPIOA, GPIO_Pin_4 | GPIO_Pin_7 | GPIO_Pin_1);
	GPIO_ResetBits(GPIOC, GPIO_Pin_5);
}
 //PIN_D -> 0-7 = K0-7, stan aktywny = 0

int main(void)
{
	RCC_config();
	GPIO_PWM_config();
	ENGINE_GPIO_config();
	ENGINE_GPIO_config2();
	GPIO_IN_config();
	PWM();
	TIMER3_config();
	TIMER2_config();

	while(1){
		if(TIM_GetFlagStatus(TIM3, TIM_FLAG_Update)) {
				    		if(e>0){
				    			e = e - 1;
				    		}
				    		TIM_ClearFlag(TIM3, TIM_FLAG_Update);
				}
		//przod
		if(GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_0)==0 && GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_1)==1 && a==0){
			przod();
			a=1;
			for(int i=0; 1000>i;i++){}
		}

		if(GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_1)==0 && GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_0)==1 && a==1){
			przod();
			a=0;
			for(int i=0; 1000>i;i++){}
		}

		//tyl
		if(GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_2)==0 && GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_3)==1 && b==0){
			tyl();
			b=1;
		}
		if(GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_3)==0 && GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_2)==1 && b==1){
			tyl();
		b=0;
		}
		//lewo
		if(GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_4)==0 && GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_5)==1 && c==0){
			lewo();
			c=1;
		}
		if(GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_5)==0 && GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_4)==1 && c==1){
			lewo();
			c=0;
		}
		//prawo
		if(GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_6)==0 && GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_7)==1 && d==0){
			prawo();
			d=1;
		}
		if(GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_7)==0 && GPIO_ReadInputDataBit(GPIOD, GPIO_Pin_6)==1 && d==1){
			prawo();
			d=0;
		}

		//stop
		if(e==0){
			stop();
		}
	}
}

