var http = require('http');

exports.request = function (config, callback) {
    var response = null;
    try {
        var request = http.Get(config.uri.href, config.headers /*"Accept-Encoding", 'gzip'*/)
        response = request.GetResponse(); //{code, message, body}
    } catch (e) {
        console.log(e.message);
        process.nextTick(function () { callback('@: ' + e.message); });
        return;
    }

    process.nextTick(function () { callback(null, null, response.body); });
}