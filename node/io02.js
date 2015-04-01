var fs = require('fs');
var file = fs.createReadStream('helloworld.js');
var newFile = fs.createWriteStream('helloworld_copy.js');

file.pipe(newFile);
