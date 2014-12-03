#include <pthread.h>
#include <stdio.h>

/* this function is run by the second thread */
void *do_work(void *n)
{
	/* increment x to 100 */
	int *n_ptr = (int *)n;
	++(*n_ptr);	
	sleep(2 + (*n_ptr));
	printf("do work finished\n");
	--(*n_ptr);
	/* the function must return something - NULL will do */
	return NULL;
}

int main()
{
	int n_threads = 0;
	int max_threads = 3;	

	/* this variable is our reference to the second thread */
	pthread_t do_work_thread1;
	pthread_t do_work_thread2;
	pthread_t do_work_thread3;

	pthread_create(&do_work_thread1, NULL, do_work, &n_threads);
	pthread_create(&do_work_thread2, NULL, do_work, &n_threads);
	pthread_create(&do_work_thread3, NULL, do_work, &n_threads);

	pthread_join(do_work_thread1, NULL);
	pthread_join(do_work_thread2, NULL);
	pthread_join(do_work_thread3, NULL);

	while(n_threads != 0){
		printf("x: %d\n", n_threads);
	}

	


	sleep(2);


	return 0;



}