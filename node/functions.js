function log(word){
	console.log(word);
}

function execute(someFunction, value){
	someFunction(value);
}

execute(log, 'hello world');
