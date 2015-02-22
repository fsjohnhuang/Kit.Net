angular.module('app', []).directive('draggable', function ($document) {
    var startX = 0, startY = 0, x = 0, y = 0;
    return function (scope, el, attr) {
        el.css({
            position: 'relative',
            border: '1px solid red',
            backgroundColor: 'lightgrey',
            cursor: 'pointer'
        });

        el.bind('mousedown', function (event) {
            console.log('st');
            startX = event.screenX - x;
            startY = event.screenY - y;
            $document.bind('mousemove', mousemove);
            $document.bind('mouseup', mouseup);
        });

        function mousemove(event) {
            x = event.screenX - startX;
            y = event.screenY - startY;

            el.css({
                top: y + 'px',
                left: x + 'px'
            });
        }

        function mouseup(event) {
            $document.unbind('mousemove', mousemove);
            $document.unbind('mouseup', mouseup);
        }
    };
});