
        (function ($) {
            $.fn.fontResize = function (options) {
                var settings = {
                    bigger: $('#increase'),
                    smaller: $('#decrease')
                };

                options = $.extend(settings, options);

                return this.each(function () {
                    var element = $(this), clicks = 0;

                    options.bigger.on('click', function (change) {
                        change.preventDefault();
                        if (clicks < 3) {
                            var baseFontSize = parseInt(element.css('font-size'));
                            var baseLineHeight = parseInt(element.css('line-height'));
                            element.css('font-size', (baseFontSize + 1) + 'px');
                            element.css('line-height', (baseLineHeight + 1) + 'px');
                            clicks += 1;
                        }
                    });

                         options.smaller.on('click', function (change) {
                        change.preventDefault();
                        if (clicks > 0) {
                            var baseFontSize = parseInt(element.css('font-size'));
                            var baseLineHeight = parseInt(element.css('line-height'));
                            element.css('font-size', (baseFontSize - 1) + 'px');
                            element.css('line-height', (baseLineHeight - 1) + 'px');
                            clicks -= 1;
                        }
                    });
                });
            };
        })(jQuery);

        $(function () {
            $('h1,p, a, table, button, input, label').fontResize(); 
        });






        var invertButton = false;


        function invertColours() {

            if (invertButton) {
                negativar2();
                invertButton = false;
                document.getElementById("coloursButton").value = "Invert Colours";
            }
            else {
                negativar();
                invertButton = true;
                document.getElementById("coloursButton").value = "Revert Colours";
            }
        }


        function negativar() {
            var percent = '85%';

            var head = document.getElementsByTagName('head')[0];
            var style = document.createElement('style');

            var css = 'html {-webkit-filter: invert(' + percent + ');' +
                '-moz-filter: invert(' + percent + ');' +
                '-o-filter: invert(' + percent + ');' +
                '-ms-filter: invert(' + percent + '); ' +
                'filter: invert(' + percent + '); ';


            style.type = 'text/css';
            if (style.styleSheet) {
                style.styleSheet.cssText = css;
            } else {
                style.appendChild(document.createTextNode(css));
            }

            head.appendChild(style);
        }

        function navegador() {
            var N = navigator.appName, ua = navigator.userAgent, tem;
            var M = ua.match(/(opera|chrome|safari|firefox|msie)\/?\s*(\.?\d+(\.\d+)*)/i);
            if (M && (tem = ua.match(/version\/([\.\d]+)/i)) != null) M[2] = tem[1];
            M = M ? [M[1], M[2]] : [N, navigator.appVersion, '-?'];
            return M;
        }

        function negativar2() {
            var percent = '0%';

            var head = document.getElementsByTagName('head')[0];
            var style = document.createElement('style');

            var css = 'html {-webkit-filter: invert(' + percent + ');' +
                '-moz-filter: invert(' + percent + ');' +
                '-o-filter: invert(' + percent + ');' +
                '-ms-filter: invert(' + percent + '); ' +
                'filter: invert(' + percent + '); ';


            style.type = 'text/css';
            if (style.styleSheet) {
                style.styleSheet.cssText = css;
            } else {
                style.appendChild(document.createTextNode(css));
            }

            head.appendChild(style);
        }

        function navegador() {
            var N = navigator.appName, ua = navigator.userAgent, tem;
            var M = ua.match(/(opera|chrome|safari|firefox|msie)\/?\s*(\.?\d+(\.\d+)*)/i);
            if (M && (tem = ua.match(/version\/([\.\d]+)/i)) != null) M[2] = tem[1];
            M = M ? [M[1], M[2]] : [N, navigator.appVersion, '-?'];
            return M;
        }
