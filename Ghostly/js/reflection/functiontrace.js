    var lookup;
    var esmorph = eval('(function (exports) { ' + process.binding('esmorph') + '\n return exports;})')({});


    function escaped(str) {
        return str;
        return str.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    }

    function isLineTerminator(ch) {
        return (ch === '\n' || ch === '\r' || ch === '\u2028' || ch === '\u2029');
    }

    exports.traceInstrument = function (code) {
        var tracer, i, functionList, signature, pos;

        tracer = esmorph.Tracer.FunctionEntrance(function (fn) {
            return 'Log.info("line: %d %s ", ' + fn.loc.start.line + ', "' + fn.name + '");';
        });

        code = esmorph.modify(code, tracer);

        // Enclose in IIFE.
        //code = '(function() {\n' + code + '\n}())';

        return code;
    };

    // by http://www.exfer.net/blog/2007/05/07/enumerating-local-variables-in-javascript/
    function indexInLiteral(str, index) {
        var inStrLiteral = 0;
        var inObjLiteral = 0;
        var inArrayLiteral = 0;

        for (var i = 0; i < str.length; i++) {
            if (inStrLiteral) {
                if (str.charAt(i) == "'" || str.charAt(i) == '"') {
                    inObjLiteral--;
                }
            } else {
                if (str.charAt(i) == "'" || str.charAt(i) == '"') {
                    inObjLiteral++;
                }
            }
            if (!inStrLiteral) {
                if (inObjLiteral) {
                    if (i == index) {
                        return (true);
                    }
                    if (str.charAt(i) == "}") {
                        inObjLiteral--;
                    }
                } else {
                    if (str.charAt(i) == "{") {
                        inObjLiteral++;
                    }
                }
                if (inArrayLiteral) {
                    if (i == index) {
                        return (true);
                    }
                    if (str.charAt(i) == "]") {
                        inArrayLiteral--;
                    }
                } else {
                    if (str.charAt(i) == "[") {
                        inArrayLiteral++;
                    }
                }
            }
        }

        return (false);
    }

    exports.getLocals = function(args) {
        var ret = [];
        var reArg = /\(?\s*(\w+)\s*(,|\))/g;
        var reEndArg = /\{/;
        var reHasVar = /(\W|^)var\W/;
        var reGetVar = /(var|,)\s*(\w+)/g;
        var lookForArgs = true;
        var reResult;
        var a;

        if (typeof args == 'function') {
            a = args.toString().split("\n");
        } else {
            a = (args) ? args.callee.toString().split("\n") : [];
        }

        ret.push(a[0]); // save the function name for later use

        for (var i = 0; i < a.length; i++) {
            if (lookForArgs) {
                reArg.lastIndex = 0;
                while ((reResult = reArg.exec(a[i])) != null) {
                    ret.push(reResult[1]);
                }
                reEndArg.lastIndex = 0;
                if (reEndArg.test(a[i])) {
                    lookForArgs = false;
                }
            }
            if (!lookForArgs) {
                reHasVar.lastIndex = 0;
                if (reHasVar.test(a[i])) {
                    reGetVar.lastIndex = 0;
                    while ((reResult = reGetVar.exec(a[i])) != null) {
                        if (!indexInLiteral(a[i], reResult.index)) {
                            ret.push(reResult[2]);
                        }
                    }
                }
            }
        }

        return (ret);
    }

    /*exports.showLocals function(func, args) {
    var str = "Variables for ";
    var arrayOfLocals = getLocals(args);
    var v;

    for (var i = 0; i < arrayOfLocals.length; i++) {
    if (i == 0) {
    str += arrayOfLocals[i] + "\n";
    } else {
    if (typeof func == 'function') {
    v = func(arrayOfLocals[i]);
    if (typeof v == 'string') {
    v = '"' + v + '"';
    }
    str += "    " + arrayOfLocals[i] + " = " + v + "\n";
    } else {
    str += "    " + arrayOfLocals[i] + "\n";
    }
    }
    }

    alert(str);
    }*/