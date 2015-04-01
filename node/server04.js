var http = require('http');
var url = require('url');

function onRequest(request, response){
	var path = url.parse(request.url).pathname;
	response.writeHead(200, {'Content-Type':'text/html'});
	request.on('data', function(chunk){
		console.log(chunk.toString());
		response.write(chunk);
	});
	request.on('end', function(){
		response.end();
	});
}

http.createServer(onRequest).listen(8888);

console.log('Listening on port 8888...');
