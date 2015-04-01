var http = require('http');

function onRequest(request, response){
	response.writeHead(200, {'Content-Type':'text/html'});
	response.write('hello world!');
	response.end();
}

http.createServer(onRequest).listen(8888);

console.log('Listening on port 8888...');
