#include "stm32f4xx.h"

int i=5;

void RCC_config(void)
{
	SystemInit();
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOA, ENABLE);
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOC, ENABLE);
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART2, ENABLE);
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM3, ENABLE);
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM2, ENABLE);
}

void TIMER3_config(void){
	TIM_TimeBaseInitTypeDef TIM_TimeBaseStructure;
	/* Time base configuration */
	TIM_TimeBaseStructure.TIM_Period =9999; //9999;
	TIM_TimeBaseStructure.TIM_Prescaler = 79; //49;
	TIM_TimeBaseStructure.TIM_ClockDivision = TIM_CKD_DIV1;
	TIM_TimeBaseStructure.TIM_CounterMode =  TIM_CounterMode_Up;
	TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
	TIM_Cmd(TIM3, ENABLE);
}

void TIMER2_config(void){
	TIM_TimeBaseInitTypeDef TIM_TimeBaseStructure;
	/* Time base configuration */
	TIM_TimeBaseStructure.TIM_Period =9999;
	TIM_TimeBaseStructure.TIM_Prescaler = 79;
	TIM_TimeBaseStructure.TIM_ClockDivision = TIM_CKD_DIV1;
	TIM_TimeBaseStructure.TIM_CounterMode =  TIM_CounterMode_Up;
	TIM_TimeBaseInit(TIM2, &TIM_TimeBaseStructure);
	TIM_Cmd(TIM2, ENABLE);
}

void PWM(void){
	TIM_OCInitTypeDef TIM_OCInitStructure;
	TIM_OCInitStructure.TIM_OCMode = TIM_OCMode_PWM1;
	TIM_OCInitStructure.TIM_OutputState = TIM_OutputState_Enable;
	TIM_OCInitStructure.TIM_Pulse = 0;
	TIM_OCInitStructure.TIM_OCPolarity = TIM_OCPolarity_High;
	TIM_OC1Init(TIM2, &TIM_OCInitStructure);
	TIM_OC1PreloadConfig(TIM2, TIM_OCPreload_Enable);
}

void GPIO_config(void)
{
		GPIO_InitTypeDef GPIO_InitStructure;
		GPIO_InitStructure.GPIO_Pin = GPIO_Pin_3 | GPIO_Pin_2;
		GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF;
		GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
		GPIO_InitStructure.GPIO_PuPd = GPIO_PuPd_UP;
		GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
		GPIO_Init(GPIOA, &GPIO_InitStructure);
		GPIO_PinAFConfig(GPIOA, GPIO_PinSource2, GPIO_AF_USART2);
		GPIO_PinAFConfig(GPIOA, GPIO_PinSource3, GPIO_AF_USART2);
}

void GPIO_PWM_config(void){
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

void USART_config(unsigned char enable_it)
{
	USART_InitTypeDef USART_InitStructure;
	USART_InitStructure.USART_BaudRate = 9600;
	USART_InitStructure.USART_WordLength = USART_WordLength_8b;
	USART_InitStructure.USART_StopBits = USART_StopBits_1;
	USART_InitStructure.USART_Parity = USART_Parity_No;
	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;
	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;
	USART_Init(USART2, &USART_InitStructure);

	USART_Cmd(USART2, ENABLE);

	if(enable_it)
	{
		 USART_ITConfig(USART2, USART_IT_RXNE, ENABLE);
	}

}

/* pin 1A,4A -> B2, B1 || pin 5C,7A -> A2, A1*/
void przod(){
	GPIO_SetBits(GPIOA, GPIO_Pin_7);
	GPIO_SetBits(GPIOA, GPIO_Pin_1);
	GPIO_ResetBits(GPIOA, GPIO_Pin_4);
	GPIO_ResetBits(GPIOC, GPIO_Pin_5);
}

void tyl(){
	GPIO_SetBits(GPIOC, GPIO_Pin_5);
	GPIO_SetBits(GPIOA, GPIO_Pin_4);
	GPIO_ResetBits(GPIOA, GPIO_Pin_7 | GPIO_Pin_1);
}

void lewo(){
	GPIO_SetBits(GPIOC, GPIO_Pin_5);
	GPIO_SetBits(GPIOA, GPIO_Pin_1);
	GPIO_ResetBits(GPIOA, GPIO_Pin_4 | GPIO_Pin_7);
}

void prawo(){
	GPIO_SetBits(GPIOA, GPIO_Pin_7);
	GPIO_SetBits(GPIOA, GPIO_Pin_4);
	GPIO_ResetBits(GPIOA, GPIO_Pin_1 );
	GPIO_ResetBits(GPIOC, GPIO_Pin_5 );
}

void stop(){
	GPIO_ResetBits(GPIOA, GPIO_Pin_4 | GPIO_Pin_7 | GPIO_Pin_1);
	GPIO_ResetBits(GPIOC, GPIO_Pin_5);
}


int main(void)
{
	RCC_config();
	GPIO_config();
	GPIO_PWM_config();
	PWM();
	ENGINE_GPIO_config();
	ENGINE_GPIO_config2();
	TIMER3_config();
	TIMER2_config();
	USART_config(1);
	TIM2->CCR1 = 6000;
	while(1){
		volatile unsigned char a;
		/////////////////////////////////////////////////////////////////
		if(TIM_GetFlagStatus(TIM3, TIM_FLAG_Update)) {
		    		if(i>0){
		    			i = i - 1;
		    		}
		    		TIM_ClearFlag(TIM3, TIM_FLAG_Update);
		}
		/////////////////////////////////////////////////////////////////
		if(USART_GetITStatus(USART2, USART_IT_RXNE) != RESET){
			USART_ClearITPendingBit(USART2, ENABLE);
			a = USART2->DR;
			i = 5;
					switch(a)
					{
					case 'a':
						lewo();
						break;
					case 'w':
						przod();
						break;
					case 's':
						tyl();
						break;
					case 'd':
						prawo();
						break;
					case '0':
						TIM2->CCR1 = 6000;
						break;
					case '1':
						TIM2->CCR1 = 6400;
						break;
					case '2':
						TIM2->CCR1 = 6800;
						break;
					case '3':
						TIM2->CCR1 = 7200;
						break;
					case '4':
						TIM2->CCR1 = 7600;
						break;
					case '5':
						TIM2->CCR1 = 8000;
						break;
					case '6':
						TIM2->CCR1 = 8400;
						break;
					case '7':
						TIM2->CCR1 = 8800;
						break;
					case '8':
						TIM2->CCR1 = 9200;
						break;
					case '9':
						TIM2->CCR1=10000;
						break;
					}
		}
		//////////////////////////////////////////////////////////////////
		if(i == 0){
			stop();
		}
		////////////////////////////////////////////////////////////////
	}
}

