(function ($global, root, $exports, $argv) {
    root.console = {
        log: $global.ConsoleLog
    };

    root.____cache = {};

    root.require = function (id) {
        if (id == 'http') {
            return $___http___;
        }

        if (root.____cache.hasOwnProperty(id))
            return $exports;

        root.____cache[id] = true;

        var source = $global.GetSource(id);

        return (function (exports) { eval(source); return exports; })($exports);
    }

    var queue = []; // Obviously this would be replaced with a fast queue in the real code

    root.process = {
        argv: $argv,
        env: {
            HOME: $global.Home,
            TMPDIR: $global.Tmpdir,
            LANG: $global.Lang
        },
        platform: $global.CurrentPlatform,

        nextTick: function nextTick(fn) {
            queue.push(fn);
        },

        // later after the tick for I/O or timers returns back to the event loop root.
        nextTick: function processNextTicks() {
            var fn
            while (fn = queue.shift()) {
                fn();
            }
        }
    };

    //root.http = {};

})($___global___, this, {}, {});

/*
process.EventEmitter.on('teste', function () {
    console.log('foi!');
});

process.EventEmitter.emit('teste');*/