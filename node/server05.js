var fs = require('fs');
var http = require('http');


function onRequest(request, response){
	var file = fs.createWriteStream('upload.txt');
	var bytes = request.headers['content-length'];
	var uploadedBytes = 0;
	var lastProgress = 0;
	request.pipe(file);
	request.on('data', function(chunk){
		uploadedBytes += chunk.length;
		var progress = parseInt((uploadedBytes / bytes) * 100, 10);
		if(progress != lastProgress){
			response.write('progress: ' + progress + '%\n');
			lastProgress = progress;
		}
	});
	request.on('end', function(){
		response.end();
	});
}

http.createServer(onRequest).listen(8888);

console.log('Listening on port 8888...');
