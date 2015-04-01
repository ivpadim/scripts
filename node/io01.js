var fs = require('fs')
var buffer = fs.readFileSync('helloworld.js')
var str = buffer.toString()

console.log(str)
