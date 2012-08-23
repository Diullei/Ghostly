// Copyright (c) 2012 by Diullei Gomes
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the
// following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
// USE OR OTHER DEALINGS IN THE SOFTWARE.

// ****************************************************************
// *************     BASED ON NODE.JS FILE     ********************
// ****************************************************************

// Copyright Joyent, Inc. and other Node contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the
// following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
// USE OR OTHER DEALINGS IN THE SOFTWARE.

var functiontrace = eval('(function (exports) { ' + process.binding('functiontrace') + '\n return exports;})')({});

var $__timerFunctionCallbackCollection__ = [];

(function (process) {

    this.global = this;
    var _console_is_loaded = false;

    global.Log = {
        level: process.env.DEBUG,

        NONE: 5,
        INFO: 4,
        DEBUG: 3,
        WARN: 2,
        ERROR: 1,
        FATAL: 0,

        CYAN: "\33[36m",
        GREEN: "\33[32m",
        YELLOW: "\33[33m",
        MAGENTA: "\33[35m",
        RED: "\33[31m",
        DEFAULT: "\33[0m",

        info: function (value, force) { this.log.call(this, force ? 100 : this.level, this.INFO, value); },
        debug: function (value, force) { this.log.call(this, force ? 100 : this.level, this.DEBUG, value); },
        warn: function (value, force) { this.log.call(this, force ? 100 : this.level, this.WARN, value); },
        error: function (value, force) { this.log.call(this, force ? 100 : this.level, this.ERROR, value); },
        fatal: function (value, force) { this.log.call(this, force ? 100 : this.level, this.FATAL, value); },

        log: function (configLevel, level, value) {
            if (!_console_is_loaded)
                return;

            if (configLevel >= 4 && level == this.INFO) console.log("%s[info]%s:  %s", this.CYAN, this.DEFAULT, value);
            else if (configLevel >= 3 && level == this.DEBUG) console.log("%s[debug]%s: %s", this.GREEN, this.DEFAULT, value);
            else if (configLevel >= 2 && level == this.WARN) console.log("%s[warn]%s:  %s", this.YELLOW, this.DEFAULT, value);
            else if (configLevel >= 1 && level == this.ERROR) console.log("%s[error]%s: %s", this.MAGENTA, this.DEFAULT, value);
            else if (configLevel >= 0 && level == this.FATAL) console.log("%s[fatal]%s: %s", this.RED, this.DEFAULT, value);
        }
    };

    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    function guid() {
        return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
    }
    function startup() {
        // ===================  basic load
        var EventEmitter = NativeModule.require('events').EventEmitter;

        process.__proto__ = Object.create(EventEmitter.prototype, {
            constructor: {
                value: process.constructor
            }
        });

        process.EventEmitter = EventEmitter; // process.EventEmitter is deprecated

        startup.globalConsole();
        startup.processStdio();

        _console_is_loaded = true;
        // ===============================

        Log.info('call - ghostly.js::startup()');

        startup.globalVariables();
        startup.globalTimeouts();
        //startup.globalConsole();

        startup.processAssert();
        // diullei
        //startup.processConfig();
        startup.processNextTick();
        startup.processMakeCallback();
        //startup.processStdio();
        startup.processKillAndExit();
        startup.processSignalHandlers();

        startup.processChannel();

        startup.resolveArgv0();

        // There are various modes that Node can run in. The most common two
        // are running from a script and running the REPL - but there are a few
        // others like the debugger or running --eval arguments. Here we decide
        // which mode we run in.

        if (NativeModule.exists('_third_party_main')) {
            // To allow people to extend Node in different ways, this hook allows
            // one to drop a file lib/_third_party_main.js into the build
            // directory which will be executed instead of Node's normal loading.
            process.nextTick(function () {
                NativeModule.require('_third_party_main');
            });

        } else if (process.argv[1] == 'debug') {
            // Start the debugger agent
            var d = NativeModule.require('_debugger');
            d.start();

        } else if (process._eval != null) {
            // User passed '-e' or '--eval' arguments to Node.
            evalScript('eval');
        } else if (process.argv[1]) {
            Log.debug('process.argv[1]');
            // make process.argv[1] into a full path
            var path = NativeModule.require('path');
            process.argv[1] = path.resolve(process.argv[1]);

            // If this is a worker in cluster mode, start up the communiction
            // channel.
            if (process.env.NODE_UNIQUE_ID) {
                var cluster = NativeModule.require('cluster');
                cluster._setupWorker();

                // Make sure it's not accidentally inherited by child processes.
                delete process.env.NODE_UNIQUE_ID;
            }

            var Module = NativeModule.require('module');

            if (global.v8debug &&
          process.execArgv.some(function (arg) {
              return arg.match(/^--debug-brk(=[0-9]*)?$/);
          })) {

                // XXX Fix this terrible hack!
                //
                // Give the client program a few ticks to connect.
                // Otherwise, there's a race condition where `node debug foo.js`
                // will not be able to connect in time to catch the first
                // breakpoint message on line 1.
                //
                // A better fix would be to somehow get a message from the
                // global.v8debug object about a connection, and runMain when
                // that occurs.  --isaacs

                setTimeout(Module.runMain, 50);

            } else {
                // REMOVEME: nextTick should not be necessary. This hack to get
                // test/simple/test-exception-handler2.js working.
                // Main entry point into most programs:
                process.nextTick(Module.runMain);
            }

        } else {
            //diullei
            /*
            var Module = NativeModule.require('module');

            // If -i or --interactive were passed, or stdin is a TTY.
            if (process._forceRepl || NativeModule.require('tty').isatty(0)) {
            // REPL
            var opts = {
            useGlobal: true,
            ignoreUndefined: false
            };
            if (parseInt(process.env['NODE_NO_READLINE'], 10)) {
            opts.terminal = false;
            }
            if (parseInt(process.env['NODE_DISABLE_COLORS'], 10)) {
            opts.useColors = false;
            }
            var repl = Module.requireRepl().start(opts);
            repl.on('exit', function () {
            process.exit();
            });

            } else {
            // Read all of stdin - execute it.
            process.stdin.resume();
            process.stdin.setEncoding('utf8');

            var code = '';
            process.stdin.on('data', function (d) {
            code += d;
            });

            process.stdin.on('end', function () {
            process._eval = code;
            evalScript('[stdin]');
            });
            }*/
        }
    }

    startup.globalVariables = function () {
        Log.info('call - ghostly.js::startup.globalVariables()');

        global.process = process;
        global.global = global;
        global.GLOBAL = global;
        global.root = global;
        //global.Buffer = NativeModule.require('buffer').Buffer;
    };

    startup.globalTimeouts = function () {
        Log.info('call - ghostly.js::startup.globalTimeouts()');

        global.setTimeout = function () {
            Log.info('call - ghostly.js::global.setTimeout()');
             var id = guid();
            $__timerFunctionCallbackCollection__[id] = arguments[0];
            $__timer__.Register(id, arguments[1], false);
        };

        global.setInterval = function () {
            Log.info('call - ghostly.js::global.setInterval()');
             var id = guid();
            $__timerFunctionCallbackCollection__[id] = arguments[0];
            $__timer__.Register(id, arguments[1], true);
        };

        global.clearTimeout = function () {
            Log.info('call - ghostly.js::global.clearTimeout()');

            var t = NativeModule.require('timers');
            return t.clearTimeout.apply(this, arguments);
        };

        global.clearInterval = function () {
            Log.info('call - ghostly.js::global.clearInterval()');

            var t = NativeModule.require('timers');
            return t.clearInterval.apply(this, arguments);
        };
    };

    startup.globalConsole = function () {
        global.__defineGetter__('console', function () {
            return NativeModule.require('console');
        });
    };

    startup._lazyConstants = null;

    startup.lazyConstants = function () {
        Log.info('call - ghostly.js::startup.lazyConstants()');

        if (!startup._lazyConstants) {
            startup._lazyConstants = process.binding('constants');
        }
        return startup._lazyConstants;
    };

    var assert;
    startup.processAssert = function () {
        Log.info('call - ghostly.js::startup.processAssert()');

        // Note that calls to assert() are pre-processed out by JS2C for the
        // normal build of node. They persist only in the node_g build.
        // Similarly for debug().
        assert = process.assert = function (x, msg) {
            if (!x) throw new Error(msg || 'assertion error');
        };
    };

    startup.processConfig = function () {
        Log.info('call - ghostly.js::startup.processConfig()');

        // used for `process.config`, but not a real module
        var config = NativeModule._source.config;
        delete NativeModule._source.config;

        // strip the gyp comment line at the beginning
        config = config.split('\n').slice(1).join('\n').replace(/'/g, '"');

        process.config = JSON.parse(config, function (key, value) {
            if (value === 'true') return true;
            if (value === 'false') return false;
            return value;
        });
    };

    startup.processMakeCallback = function () {
        Log.info('call - ghostly.js::startup.processMakeCallback()');

        process._makeCallback = function (obj, fn, args) {
            Log.info('call - ghostly.js::process._makeCallback()');

            var domain = obj.domain;
            if (domain) {
                if (domain._disposed) return;
                domain.enter();
            }

            var ret = fn.apply(obj, args);

            if (domain) domain.exit();

            // process the nextTicks after each time we get called.
            process._tickCallback();
            return ret;
        };
    };

    //var queue = [];

    startup.processNextTick = function () {
        Log.info('call - ghostly.js::startup.processNextTick()');

        var nextTickQueue = [];
        var nextTickIndex = 0;
        var inTick = false;
        var tickDepth = 0;

        process._needTickCallback = function () {
            Log.info('call - ghostly.js::process._needTickCallback()');

            var fn
            while (fn = nextTickQueue.shift()) {
                fn.callback();
            }
        };

        // the maximum number of times it'll process something like
        // nextTick(function f(){nextTick(f)})
        // It's unlikely, but not illegal, to hit this limit.  When
        // that happens, it yields to libuv's tick spinner.
        // This is a loop counter, not a stack depth, so we aren't using
        // up lots of memory here.  I/O can sneak in before nextTick if this
        // limit is hit, which is not ideal, but not terrible.
        process.maxTickDepth = 1000;

        function tickDone(tickDepth_) {
            Log.info('call - ghostly.js::startup.processNextTick.tickDone(%s)', tickDepth_);

            tickDepth = tickDepth_ || 0;
            nextTickQueue.splice(0, nextTickIndex);
            nextTickIndex = 0;
            inTick = false;
            if (nextTickQueue.length) {
                process._needTickCallback();
            }
        }

        process._tickCallback = function (fromSpinner) {
            Log.info('call - ghostly.js::process._tickCallback(%s)', fromSpinner);

            // if you add a nextTick in a domain's error handler, then
            // it's possible to cycle indefinitely.  Normally, the tickDone
            // in the finally{} block below will prevent this, however if
            // that error handler ALSO triggers multiple MakeCallbacks, then
            // it'll try to keep clearing the queue, since the finally block
            // fires *before* the error hits the top level and is handled.
            if (tickDepth >= process.maxTickDepth) {
                if (fromSpinner) {
                    // coming in from the event queue.  reset.
                    tickDepth = 0;
                } else {
                    if (nextTickQueue.length) {
                        process._needTickCallback();
                    }
                    return;
                }
            }

            if (!nextTickQueue.length) return tickDone();

            if (inTick) return;
            inTick = true;

            // always do this at least once.  otherwise if process.maxTickDepth
            // is set to some negative value, or if there were repeated errors
            // preventing tickDepth from being cleared, we'd never process any
            // of them.
            do {
                tickDepth++;
                var nextTickLength = nextTickQueue.length;
                if (nextTickLength === 0) return tickDone();
                while (nextTickIndex < nextTickLength) {
                    var tock = nextTickQueue[nextTickIndex++];
                    var callback = tock.callback;
                    if (tock.domain) {
                        if (tock.domain._disposed) continue;
                        tock.domain.enter();
                    }
                    var threw = true;
                    try {
                        callback();
                        threw = false;
                    } finally {
                        // finally blocks fire before the error hits the top level,
                        // so we can't clear the tickDepth at this point.
                        if (threw) tickDone(tickDepth);
                    }
                    if (tock.domain) {
                        tock.domain.exit();
                    }
                }
                nextTickQueue.splice(0, nextTickIndex);
                nextTickIndex = 0;

                // continue until the max depth or we run out of tocks.
            } while (tickDepth < process.maxTickDepth &&
        nextTickQueue.length > 0);

            tickDone();
        };

        process.nextTick = function (callback) {
            Log.info('call - ghostly.js::process.nextTick(%s)', callback);

            // on the way out, don't bother.
            // it won't get fired anyway.
            if (process._exiting) return;

            var tock = { callback: callback };
            if (process.domain) tock.domain = process.domain;
            nextTickQueue.push(tock);
            if (nextTickQueue.length) {
                process._needTickCallback();
            }
        };
    };

    function evalScript(name) {
        Log.info('call - ghostly.js::evalScript(%s)', name);

        var Module = NativeModule.require('module');
        var path = NativeModule.require('path');
        var cwd = process.cwd();

        var module = new Module(name);
        module.filename = path.join(cwd, name);
        module.paths = Module._nodeModulePaths(cwd);
        var result = module._compile('return eval(process._eval)', name);
        if (process._print_eval) console.log(result);
    }

    function errnoException(errorno, syscall) {
        Log.info('call - ghostly.js::errnoException(%s, %s)', errorno, syscall);

        // TODO make this more compatible with ErrnoException from src/node.cc
        // Once all of Node is using this function the ErrnoException from
        // src/node.cc should be removed.
        var e = new Error(syscall + ' ' + errorno);
        e.errno = e.code = errorno;
        e.syscall = syscall;
        return e;
    }

    function createWritableStdioStream(fd) {
        process.console('[info]:  call - ghostly.js::createWritableStdioStream(' + fd + ')');

        var stream;
        var tty_wrap = NativeModule.require('tty_wrap');

        // Note stream._type is used for test-module-load-list.js

        // diullei
        switch (tty_wrap.guessHandleType(fd)) {
            //switch ('TTY') {                     
            case 'TTY':
                var tty = NativeModule.require('tty');
                stream = new tty.WriteStream(fd);
                stream._type = 'tty';

                // Hack to have stream not keep the event loop alive.
                // See https://github.com/joyent/node/issues/1726
                if (stream._handle && stream._handle.unref) {
                    stream._handle.unref();
                }
                process.console('TTY end');
                break;

            case 'FILE':
                var fs = NativeModule.require('fs');
                stream = new fs.SyncWriteStream(fd);
                stream._type = 'fs';
                break;

            case 'PIPE':
                var net = NativeModule.require('net');
                stream = new net.Stream(fd);

                // FIXME Should probably have an option in net.Stream to create a
                // stream from an existing fd which is writable only. But for now
                // we'll just add this hack and set the `readable` member to false.
                // Test: ./node test/fixtures/echo.js < /etc/passwd
                stream.readable = false;
                stream._type = 'pipe';

                // FIXME Hack to have stream not keep the event loop alive.
                // See https://github.com/joyent/node/issues/1726
                if (stream._handle && stream._handle.unref) {
                    stream._handle.unref();
                }
                break;

            default:
                // Probably an error on in uv_guess_handle()
                throw new Error('Implement me. Unknown stream file type!');
        }

        // For supporting legacy API we put the FD here.
        stream.fd = fd;

        stream._isStdio = true;

        return stream;
    }

    startup.processStdio = function () {
        var stdin, stdout, stderr;

        process.__defineGetter__('stdout', function () {
            if (stdout) return stdout;
            stdout = createWritableStdioStream(1);
            stdout.destroy = stdout.destroySoon = function (er) {
                er = er || new Error('process.stdout cannot be closed.');
                stdout.emit('error', er);
            };
            if (stdout.isTTY) {
                process.on('SIGWINCH', function () {
                    stdout._refreshSize();
                });
            }
            return stdout;
        });

        /*
        process.__defineGetter__('stderr', function () {
        if (stderr) return stderr;

        process.console('[info]:  call - ghostly.js::process.__defineGetter__(stderr)');
        stderr = createWritableStdioStream(2);
        stderr.destroy = stderr.destroySoon = function (er) {
        er = er || new Error('process.stderr cannot be closed.');
        stderr.emit('error', er);
        };
        return stderr;
        });*/

        /*process.__defineGetter__('stdin', function () {
        if (stdin) return stdin;

        process.console('[info]:  call - ghostly.js::process.__defineGetter__(stdin)');
        var tty_wrap = process.binding('tty_wrap');
        var fd = 0;

        switch (tty_wrap.guessHandleType(fd)) {
        case 'TTY':
        var tty = NativeModule.require('tty');
        stdin = new tty.ReadStream(fd);
        break;

        case 'FILE':
        var fs = NativeModule.require('fs');
        stdin = new fs.ReadStream(null, { fd: fd });
        break;

        case 'PIPE':
        var net = NativeModule.require('net');
        stdin = new net.Stream(fd);
        stdin.readable = true;
        break;

        default:
        // Probably an error on in uv_guess_handle()
        throw new Error('Implement me. Unknown stdin file type!');
        }

        // For supporting legacy API we put the FD here.
        stdin.fd = fd;

        // stdin starts out life in a paused state, but node doesn't
        // know yet.  Call pause() explicitly to unref() it.
        stdin.pause();

        // when piping stdin to a destination stream,
        // let the data begin to flow.
        var pipe = stdin.pipe;
        stdin.pipe = function (dest, opts) {
        stdin.resume();
        return pipe.call(stdin, dest, opts);
        };

        return stdin;
        });*/

        process.openStdin = function () {
            process.stdin.resume();
            return process.stdin;
        };
    };

    startup.processKillAndExit = function () {
        Log.info('call - ghostly.js::startup.processKillAndExit()');

        process.exit = function (code) {
            Log.info('call - ghostly.js::process.exit(%s)', code);

            if (!process._exiting) {
                process._exiting = true;
                process.emit('exit', code || 0);
            }
            process.reallyExit(code || 0);
        };

        process.kill = function (pid, sig) {
            Log.info('call - ghostly.js::process.kill(%s, %s)', pid, sig);
            var r;

            // preserve null signal
            if (0 === sig) {
                r = process._kill(pid, 0);
            } else {
                sig = sig || 'SIGTERM';
                if (startup.lazyConstants()[sig]) {
                    r = process._kill(pid, startup.lazyConstants()[sig]);
                } else {
                    throw new Error('Unknown signal: ' + sig);
                }
            }

            if (r) {
                throw errnoException(errno, 'kill');
            }

            return true;
        };
    };

    startup.processSignalHandlers = function () {
        Log.info('call - ghostly.js::startup.processSignalHandlers()');

        // Load events module in order to access prototype elements on process like
        // process.addListener.
        var signalWatchers = {};
        var addListener = process.addListener;
        var removeListener = process.removeListener;

        function isSignal(event) {
            Log.info('call - ghostly.js::startup.processSignalHandlers.isSignal(%d)', event);

            return event.slice(0, 3) === 'SIG' && startup.lazyConstants()[event];
        }

        // Wrap addListener for the special signal types
        process.on = process.addListener = function (type, listener) {
            Log.info('call - ghostly.js::process.on = process.addListener(%s, %s)', type, listener);

            var ret = addListener.apply(this, arguments);
            if (isSignal(type)) {
                if (!signalWatchers.hasOwnProperty(type)) {
                    var b = process.binding('signal_watcher');
                    var w = new b.SignalWatcher(startup.lazyConstants()[type]);
                    w.callback = function () { process.emit(type); };
                    signalWatchers[type] = w;
                    w.start();

                } else if (this.listeners(type).length === 1) {
                    signalWatchers[type].start();
                }
            }

            return ret;
        };

        process.removeListener = function (type, listener) {
            Log.info('call - ghostly.js::process.removeListener(%s, %s)', type, listener);

            var ret = removeListener.apply(this, arguments);
            if (isSignal(type)) {
                assert(signalWatchers.hasOwnProperty(type));

                if (this.listeners(type).length === 0) {
                    signalWatchers[type].stop();
                }
            }

            return ret;
        };
    };


    startup.processChannel = function () {
        Log.info('call - ghostly.js::startup.processChannel()');

        // If we were spawned with env NODE_CHANNEL_FD then load that up and
        // start parsing data from that stream.
        if (process.env.NODE_CHANNEL_FD) {
            var fd = parseInt(process.env.NODE_CHANNEL_FD, 10);
            assert(fd >= 0);

            // Make sure it's not accidentally inherited by child processes.
            delete process.env.NODE_CHANNEL_FD;

            var cp = NativeModule.require('child_process');

            // Load tcp_wrap to avoid situation where we might immediately receive
            // a message.
            // FIXME is this really necessary?
            process.binding('tcp_wrap');

            cp._forkChild(fd);
            assert(process.send);
        }
    }

    startup.resolveArgv0 = function () {
        Log.info('call - ghostly.js::startup.resolveArgv0()');

        var cwd = process.cwd();
        var isWindows = process.platform === 'win32';

        // Make process.argv[0] into a full path, but only touch argv[0] if it's
        // not a system $PATH lookup.
        // TODO: Make this work on Windows as well.  Note that "node" might
        // execute cwd\node.exe, or some %PATH%\node.exe on Windows,
        // and that every directory has its own cwd, so d:node.exe is valid.
        var argv0 = process.argv[0];
        if (!isWindows && argv0.indexOf('/') !== -1 && argv0.charAt(0) !== '/') {
            var path = NativeModule.require('path');
            process.argv[0] = path.join(cwd, process.argv[0]);
        }
    };

    // Below you find a minimal module system, which is used to load the node
    // core modules found in lib/*.js. All core modules are compiled into the
    // node binary, so they can be loaded faster.

    var Script = process.binding('evals').NodeScript;
    var runInThisContext = Script.runInThisContext;

    function NativeModule(id) {
        this.filename = id + '.js';
        this.id = id;
        this.exports = {};
        this.loaded = false;
    }

    NativeModule._source = process.binding('natives');
    NativeModule._cache = {};

    NativeModule.require = function (id) {
        if (process.env.INFO_REQUIRE)
            if (id != 'console') process.console('[info]:  call - ghostly.js::NativeModule.require(' + id + ')');

        if (id == 'http') {
            return $___http___;
        }

        if (id == 'native_module') {
            return NativeModule;
        }

        var cached = NativeModule.getCached(id);
        if (cached) {
            return cached.exports;
        }

        if (!NativeModule.exists(id)) {
            throw new Error('No such native module ' + id);
        }

        process.moduleLoadList.push('NativeModule ' + id);

        var nativeModule = new NativeModule(id);

        nativeModule.compile();
        nativeModule.cache();

        nativeModule.parent = this;

        return nativeModule.exports;
    };

    NativeModule.getCached = function (id) {
        return NativeModule._cache[id];
    }

    NativeModule.exists = function (id) {
        return NativeModule._source.hasOwnProperty(id);
    }

    NativeModule.getSource = function (id) {
        return NativeModule._source[id];
    }

    NativeModule.wrap = function (script) {
        return NativeModule.wrapper[0] + script + NativeModule.wrapper[1];
    };

    NativeModule.wrapper = [
    '(function (exports, require, module, __filename, __dirname) { ',
    '\n});'
  ];

    NativeModule.prototype.compile = function () {
        var source = NativeModule.getSource(this.id);
        source = NativeModule.wrap(source);

        var fn = runInThisContext(source, this.filename, true);
        //fn(this.exports, NativeModule.require, this, this.filename);
        ///
//        if(this.id == 'JScript1.js')
//        {
//            Log.debug(':::::::');
//            var source = functiontrace.traceInstrument(required.source)
//        }
//        ///
        eval(source)(this.exports, NativeModule.require, this, this.filename);

        this.loaded = true;
    };

    NativeModule.prototype.cache = function () {
        NativeModule._cache[this.id] = this;
    };

    startup();

    process.NativeModule = NativeModule;
})(process);

(function (root) { 
    function ModuleLoader(id, dir) {
        this.filename = id + '.js';
        this.id = id;
        this.dir = dir;
        this.fullId = process.getRealPath(id, dir);
        this.exports = {};
        this.loaded = false;
    }

    ModuleLoader._cache = {};

    ModuleLoader.require = function (id, dir) { 
        if (process.env.INFO_REQUIRE)
            Log.info('call - ghostly.js::ModuleLoader.require(' + id + ', ' + dir + ')');

        if (id == 'http') {
            return $___http___;
        }

        var key = process.getRealPath(id, dir);
        //process.console('key: ' + key);
        var cached = ModuleLoader.getCached(key);
        if (cached) {
            return cached.exports;
        }

        var module = new ModuleLoader(id, dir);

        module.compile();
        module.cache();

		module.parent = this;
		
        return module.exports;
    }

    ModuleLoader.getCached = function (id) {
        return ModuleLoader._cache[id];
    }

    ModuleLoader.wrapper = [
        '(function (exports, require, module, __filename, __dirname) { ',
        '\n});'
    ];

    ModuleLoader.getSource = function (id, dir) {
        return process.require(id, dir);
    }

    ModuleLoader.wrap = function (script) {
        return ModuleLoader.wrapper[0] + script + ModuleLoader.wrapper[1];
    };

    ModuleLoader.prototype.compile = function () {
        var required = ModuleLoader.getSource(this.id, this.dir);
        this.dir = required.dirname;
        ///
        ///var source = functiontrace.traceInstrument(required.source)
        ///
        source = ModuleLoader.wrap(required.source);

        try {
            eval(source)(
                this.exports, 
                function(id){ 
                    try {
                        return process.NativeModule.require(id);
                    } catch (e) {
                        return (function (id) { return ModuleLoader.require(id, this.dirname); }).call(required, id);
                    }
                },
                this, 
                this.filename);
        } catch (e) {
            Log.fatal('compile module(' + this.id + ') - Error: ' + e.message);
            Log.fatal('compile module(' + this.id + ') - Stack: ' + e.stack);
            throw e;
        }

        this.loaded = true;
    };

    ModuleLoader.prototype.cache = function () {
        ModuleLoader._cache[this.fullId] = this;
    };

    root.require = function(id, dir){ 
        try {
            return process.NativeModule.require(id);
        } catch (e) {
            return (function(id, dir){ return ModuleLoader.require(id, dir); }).call({}, id, dir);
        }
    };

})(this);
