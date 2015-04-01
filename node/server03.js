var http = require('http');
var url = require('url');

function route(path){
	console.log('requesting path ' + path);
}

function onRequest(request, response){
	var path = url.parse(request.url).pathname;
	route(path);
	response.writeHead(200, {'Content-Type':'text/html'});
	response.write('hello world!');
	response.end();
}

http.createServer(onRequest).listen(8888);

console.log('Listening on port 8888...');
