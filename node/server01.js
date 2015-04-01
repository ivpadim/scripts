var http = require('http');

http.createServer(function(request, response) {
	response.writeHead(200, {'Content-Type': 'text/html'});
	response.write('hello world!');
	response.end();
}).listen(8888);

console.log('Listening on port 8888...');
