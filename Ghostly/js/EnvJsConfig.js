var log = {
    debug: function(msg){ console.log(msg); },
    info: function(msg){ console.log(msg); },
    warn: function(msg){ console.log(msg); }
}

var __context__ = __this__ = this;

var Envjs = Envjs || 
	require('envjs/platform/core').Envjs;
	//require('local_settings');

Envjs.platform       = "Ghostly";
Envjs.revision       = $___global___.Version;

/*process.on('uncaughtException', function (err) {
  	console.log('Envjs Caught exception: %s \n %s', err);
});*/


Envjs.argv = process.argv;
//Envjs.argv.shift();
//Envjs.argv.shift();//node is argv[0] but we want to start at argv[1]

Envjs.exit = function(){
    /*setTimeout(function () {
        if(!Envjs.timers.length){
            //console.log('no timers remaining %s', Envjs.timers.length);
            process.exit();
        }else{
            Envjs.exit();
        }
    }, 13);*/
};


/*
 * Envjs node-env.1.3.pre03 
 * Pure JavaScript Browser Environment
 * By John Resig <http://ejohn.org/> and the Envjs Team
 * Copyright 2008-2010 John Resig, under the MIT License
 */

//CLOSURE_START
(function(){





/**
 * @author john resig
 */
// Helper method for extending one object with another.
function __extend__(a,b) {
    for ( var i in b ) {
        if(b.hasOwnProperty(i)){
            var g = b.__lookupGetter__(i), s = b.__lookupSetter__(i);
            if ( g || s ) {
                if ( g ) { a.__defineGetter__(i, g); }
                if ( s ) { a.__defineSetter__(i, s); }
            } else {
                a[i] = b[i];
            }
        }
    } 
    return a;
}

//var $print = require('sys').print;

Envjs.log = function(msg){
    //console.log(msg+'\n\n');
    $___global___.ConsoleLog(msg+'\n\n');
};

Envjs.lineSource = function(e){
    return "(line ?)";
};

Envjs.readConsole = function(){
    console.log('Envjs readConsole Not Available');
    return;// $stdin.gets();
};

Envjs.prompt = function(){
    console.log('Envjs prompt Not Available');
    //$stdout.write(Envjs.CURSOR+' ');
    //$stdout.flush;
};

//No REPL for
Envjs.repl = function(){
    //require('repl').start(Envjs.CURSOR+' ');
    console.log('Envjs REPL Not Available');
};
//var Script = process.binding('evals').Script;

Envjs.eval = function(context, source, name, warming){
    return eval(source);
	/*if(context === global){
		return warming ? 
			eval(source) :
			Script.runInThisContext(source, name);
	}else{
		return Script.runInNewContext(source, context, name);
	}*/
};
Envjs.wait = function(){ return; }

var $tick = function(){
    process.nextTick(function () {
        //console.log('node tick');
        Envjs.tick();
        $tick();
    });
};
    
Envjs.eventLoop = function(){
    //console.log('event loop');
    $tick();
};
/**
 * provides callback hook for when the system exits
 */
Envjs.onExit = function(callback){
    process.on('exit', callback);
};


/**
 * synchronizes thread modifications
 * @param {Function} fn
 */
 Envjs.sync = function(fn){
     console.log('syncing js fn %s', fn);
     return fn;
 };

 Envjs.spawn = function(fn){
     return fn();
 };

/**
 * sleep thread for specified duration
 * @param {Object} milliseconds
 */
Envjs.sleep = function(milliseconds){
    return;
};


//Since we're running in v8 I guess we can safely assume
//java is not 'enabled'.  I'm sure this requires more thought
//than I've given it here
Envjs.javaEnabled 	 = false;

Envjs.homedir        = process.env["HOME"];
Envjs.tmpdir         = process.env["TMPDIR"];
Envjs.os_name        = process.platform;
Envjs.os_arch        = "os arch";
Envjs.os_version     = "os version";
Envjs.lang           = process.env["LANG"];


Envjs.gc = function(){ return; };

/**
 * Makes an object window-like by proxying object accessors
 * @param {Object} scope
 * @param {Object} parent
 */
Envjs.proxy = function(scope, parent) {
    try{
        if(scope.toString() == global){
            return __this__;
        }else{
            return scope;
        }
    }catch(e){
       console.log('failed to init standard objects %s %s \n%s', scope, parent, e);
    }

};
//var filesystem = require('fs');
/**
 * Get 'Current Working Directory'
 */
Envjs.getcwd = function() {
    return process.cwd();
}


/**
 * Used to write to a local file
 * @param {Object} text
 * @param {Object} url
 */
Envjs.writeToFile = function(text, url){ colsole.log('Envjs.writeToFile');
	if(/^file\:\/\//.test(url))
		url = url.substring(7,url.length);
    filesystem.writeFileSync(url, text, 'utf8');
};

/**
 * Used to write to a local file
 * @param {Object} text
 * @param {Object} suffix
 */
Envjs.writeToTempFile = function(text, suffix){ colsole.log('Envjs.writeToTempFile');
	var url =  Envjs.tmpdir+'envjs-'+(new Date().getTime())+'.'+suffix;
	if(/^file\:\/\//.test(url))
		url = url.substring(7,url.length);
	filesystem.writeFileSync(tmpfile, text, 'utf8');
    return tmpfile;
};


/**
 * Used to read the contents of a local file
 * @param {Object} url
 */
Envjs.readFromFile = function( url ){ colsole.log('Envjs.readFromFile');
	if(/^file\:\/\//.test(url))
		url = url.substring(7,url.length);
    return filesystem.readFileSync(url, 'utf8');
};
    

/**
 * Used to delete a local file
 * @param {Object} url
 */
Envjs.deleteFile = function(url){ colsole.log('Envjs.deleteFile');
	if(/^file\:\/\//.test(url))
		url = url.substring(7,url.length);
    filesystem.unlink(url);
};

/**
 * establishes connection and calls responsehandler
 * @param {Object} xhr
 * @param {Object} responseHandler
 * @param {Object} data
 */
Envjs.connection = function(xhr, responseHandler, data){
    var url = xhr.url,
		urlparts = Envjs.urlsplit(xhr.url),
        connection,
		request,
		headers,
        header,
        length,
        binary = false,
        name, value,
        contentEncoding,
        responseXML,
        i;
       
    if ( /^file\:/.test(url) ) {
        Envjs.localXHR(url, xhr, connection, data);
    } else {
	    log.debug('establishing http connection %s', xhr.url);
		try{
			
	        /*connection = Ruby.Net.HTTP.start(
				urlparts.hostname,
				Number(urlparts.port||80)
			);*/
	        console.log('connection established %s');
			//log.debug('connection established %s', connection);

            var http = require('http');
	                console.log('http');

			path = urlparts.path+(urlparts.query?'?'+urlparts.query:'');

            //var p_url = 'http://' + urlparts.hostname + ':' + urlparts.port||80 + path;

		    switch(	xhr.method.toUpperCase() ){
				case "GET": 
	                console.log('GET');
					//request = new Ruby.Net.HTTP.Get(path);break;
                    request = new http.Get(urlparts.hostname, (urlparts.port||80), path, headers /*"Accept-Encoding", 'gzip'*/);break;
			    case "PUT":
					request = new Ruby.Net.HTTP.Put(path);break;
			    case "POST"	:
					request = new Ruby.Net.HTTP.Post(path);break;
			    case "HEAD":
					request = new Ruby.Net.HTTP.Head(path);break;
			    case "DELETE":
					request = new Ruby.Net.HTTP.Delete(path);break;
				default:
					request = null;
			}
	        console.log('request query string %s');
	        console.log('formulated %s request %s %s');
			//log.debug('request query string %s', urlparts.query);
			//log.debug('formulated %s request %s %s', xhr.method, urlparts.path, request);
			//TODO: add gzip support
	        //connection.putheader("Accept-Encoding", 'gzip');
			//connection.endheaders();
		
	        //write data to output stream if required
			//TODO: if they have set the request header for a chunked
			//request body, implement a chunked output stream
            console.log('data: ' + data);
	        if(data){
	            if(data instanceof Document){
	                if ( xhr.method == "PUT" || xhr.method == "POST" ) {
                        console.log('xhr.method == "PUT" || xhr.method == "POST"');
	                    //connection.send((new XMLSerializer()).serializeToString(data));
                        request.send((new XMLSerializer()).serializeToString(data));
	                }
	            }else if(data.length&&data.length>0){
	                if ( xhr.method == "PUT" || xhr.method == "POST" ) {
                        console.log('xhr.method == "PUT" || xhr.method == "POST"');
	                    //connection.send(data+'');
                        request.send(data+'');
	                }
	            }
	        }
		}catch(e){
			log.error("connection failed %s",e);
			//if (connection)
			//	connection.finish();
			//connection = null;
		}
    }

    console.log('testando conexão');
    //if(connection)
    {
		//[response, headers] = HTTPConnection.go(connection, request, xhr.headers, null);
		var response = request.GetResponse();
        console.log(response);
        console.log('pegou response!!!');
        console.log('xhr: ' + xhr);
        try{
            // Stick the response headers into responseHeaders
			var keys = headers.keys();
            for (var i = 0;i<keys.length;i++) {
				header = keys[i];
				log.debug('response header [%s] = %s', header, headers[header]);
                xhr.responseHeaders[header] = headers[header];
            }
        }catch(e){
            console.log('failed to load response headers \n%s');
            //log.error('failed to load response headers \n%s',e);
        }
        console.log('*');

        try {
            xhr.readyState = 4;
            console.log('foi! ' + xhr.readyState);
        } catch (e) {
            console.log('não foi');
        }

            console.log('*response.code: ' + response.code);
            console.log('*response.message: ' + response.message);
        xhr.status = parseInt(response.code,10) || undefined;
        xhr.statusText = response.message || "";
            console.log('*xhr.statusText: ' + xhr.statusText);
        console.log('%s %s %s %s');
		//log.info('%s %s %s %s', xhr.method, xhr.status, xhr.url, xhr.statusText);
        contentEncoding = xhr.getResponseHeader('content-encoding') || "utf-8";
        responseXML = null;
        
            console.log('response.code: ' + response.code);
            console.log('response.message: ' + response.message);

        try{
            //console.log('contentEncoding %s', contentEncoding);
            if( contentEncoding.toLowerCase() == "gzip" ||
                contentEncoding.toLowerCase() == "decompress"){
                //zipped content
                binary = true;
				//TODO
				log.debug('TODO: handle gzip compression');
                xhr.responseText = response.body;
            }else{
                //this is a text file
                xhr.responseText = response.body+'';
            }
        }catch(e){
            log.warn('failed to open connection stream \n%s, %s %s %s',
            	xhr.status, xhr.url, e.toString(), e
			);
        }finally{
			if(connection)
				connection.finish();
		}


    }
    
    if(responseHandler){
        log.debug('calling ajax response handler');
        if(!xhr.async){
			log.debug('calling sync response handler directly');
			responseHandler();
		}else{
			log.debug('using setTimeout for async response handler');
			setTimeout(responseHandler, 1);
		}
    }

    console.log('FIM!!!!');
}; /*
Envjs.connection = function(xhr, responseHandler, data){
	
    var url = xhr.url,
        connection,
		request,
        binary = false,
        contentEncoding,
        responseXML = null,
		http = require('http'),
		urlparts = Envjs.urlsplit(url),
        i;

        console.log('debug0: ' + url);
        
    if ( /^file\:/.test(url) ) {
        console.log('debug: ' + url);
        Envjs.localXHR(url, xhr, connection, data);
    } else {
        console.log('debug1: ' + url);
	    //console.log('connecting to %s \n\t port(%s) host(%s) path(%s) query(%s)', 
	    //    url, urlparts.port, urlparts.hostname, urlparts.path, urlparts.query);
		connection = http.createClient(urlparts.port||'80', urlparts.hostname);
		request = connection.request(
			xhr.method, 
			urlparts.path+(urlparts.query?"?"+urlparts.query:''),
			__extend__(xhr.headers,{
				"Host": urlparts.hostname,
				//"Connection":"Keep-Alive"
				//"Accept-Encoding", 'gzip'
			})
		);
		xhr.statusText = "";

        //////////////////////
            
//		xhr.readyState = 3;
//        request.response()
//		xhr.readyState = 4;
//		if(responseHandler){
//			//console.log('calling ajax response handler');
//			if(!xhr.async){
//				responseHandler();
//			}else{
//				setTimeout(responseHandler, 1);
//			}
//		}
//		if(xhr.onreadystatechange){
//			xhr.onreadystatechange();
//		}

        /////////////////

	    if(connection&&request){
			request.on('response', function (response) {
				//console.log('response begin');
				xhr.readyState = 3;
				response.on('end', function (chunk) {
					//console.log('connection complete');
		        	xhr.readyState = 4;
				    if(responseHandler){
				        //console.log('calling ajax response handler');
				        if(!xhr.async){
							responseHandler();
						}else{
							setTimeout(responseHandler, 1);
						}
				    }
					if(xhr.onreadystatechange){
						xhr.onreadystatechange();
					}
				});

				xhr.responseHeaders = __extend__({},response.headers);
                xhr.statusText = "OK";
			    xhr.status = response.statusCode;
				//console.log('response headers : %s', JSON.stringify(xhr.responseHeaders));
				contentEncoding = xhr.getResponseHeader('Content-Encoding') || "utf-8";
				response.setEncoding(contentEncoding);
	            //console.log('contentEncoding %s', contentEncoding);

			  	response.on('data', function (chunk) {
			    	//console.log('\nBODY: %s', chunk);
		            if( contentEncoding.match("gzip") ||
		                contentEncoding.match("decompress")){
		                //zipped content
		                binary = true;
						//Not supported yet
		                xhr.responseText += (chunk+'');
		            }else{
		                //this is a text file
		                xhr.responseText += (chunk+'');
		            }
					if(xhr.onreadystatechange){
						xhr.onreadystatechange();
					}
			  	});
				if(xhr.onreadystatechange){
					xhr.onreadystatechange();
				}
			});
	    }
        //write data to output stream if required
		//TODO: if they have set the request header for a chunked
		//request body, implement a chunked output stream
		
        //console.log('sending request %s\n', xhr.url);
        if(data){
            if(data instanceof Document){
                if ( xhr.method == "PUT" || xhr.method == "POST" ) {
                    xml = (new XMLSerializer()).serializeToString(data);
					request.write(xml);
                }
            }else if(data.length&&data.length>0){
                if ( xhr.method == "PUT" || xhr.method == "POST" ) {
                    request.write(data);
                }
            }
            request.end();
        }else{
            request.end();
        }
    }

};
*/

/**
 * @author john resig & the envjs team
 * @uri http://www.envjs.com/
 * @copyright 2008-2010
 * @license MIT
 */
//CLOSURE_END
}());
