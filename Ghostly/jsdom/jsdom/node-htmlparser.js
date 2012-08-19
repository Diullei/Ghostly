var htmlparser = require("./htmlparser");

console.log(htmlparser);

exports.Parser = htmlparser.Parser;
exports.DefaultHandler = htmlparser.DefaultHandler;
exports.RssHandler = htmlparser.RssHandler;
exports.ElementType = htmlparser.ElementType;
exports.DomUtils = htmlparser.DomUtils;
