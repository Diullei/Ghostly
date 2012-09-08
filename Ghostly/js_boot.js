var page = require('webpage').create();

page.settings.loadImages = false;

page.onAlert = function (msg) { console.log(msg); };

page.onConsoleMessage = function (msg) {
    // Encerra o phantom se o processo principal terminar.
    if (msg == 'COM REQUEST FAIL: 0')
        phantom.exit();

    console.log(msg);
};

var count = 0;
page.onLoadFinished = function () {
    (0 == count++) && page.evaluate(function () {
        var i = document.createElement('iframe');
        i.src = '###=URL=###';
        i.onload = function () {
            console.log('page loaded...');
            var $wnd = this.contentWindow;

            function getScript() {
                console.log('.');
                $.ajax({
                    url: '/com',
                    type: 'GET',
                    success: function (data) {
                        console.log(data);
                        eval('data = ' + data);

                        try {
                            var res = null;
                            eval('res = $wnd.' + data.script);
                            $.get('/callback?id=' + data.id + '&result=' + res);
                            //$.post('/callback', { id: data.split('|')[0], result: res });
                        } catch (e) {
                            $.get('/callback?id=' + data.id + '&result=');
                        }
                    },
                    error: function (e) {
                        console.log('COM REQUEST FAIL: ' + e.status);
                    }
                });
            }

            $.get('/onload');

            getScript();
            setInterval(getScript, 500);
        };
        document.body.appendChild(i);
    });
};


page.open('http://localhost:###=PORT=###/main.html');