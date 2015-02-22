(function() {
  var Ajax;

  Ajax = (function() {
    var arg, xhrCtor, xhrQueue, _getXHR, _ref;

    function Ajax() {}

    _ref = window.XMLHttpRequest != null ? [window.XMLHttpRequest, null] : [ActiveXObject, 'Microsoft.XMLHTTP'], xhrCtor = _ref[0], arg = _ref[1];

    xhrQueue = (function() {
      var i, _xhrQueue;

      return _xhrQueue = (function() {
        var _i, _results;

        _results = [];
        for (i = _i = 0; _i <= 3; i = ++_i) {
          _results.push(new xhrCtor(arg));
        }
        return _results;
      })();
    })();

    _getXHR = function() {
      if (xhrQueue.length >= 1) {
        return xhrQueue.pop();
      } else {
        return new xhrCtor(arg);
      }
    };

    Ajax.prototype.req = function(url, callback, state, method, postData) {
      var xhr;

      if (lpp.isEmpty(url)) {
        return;
      }
      if (method == null) {
        method = 'GET';
      }
      xhr = _getXHR();
      xhr.onreadystatechange = function() {
        switch (xhr.readyState) {
          case 1:
            return lpp.log('XHR is opened!');
          case 2:
            return lpp.log('XHR is on preload!');
          case 3:
            return lpp.log('XHR is on loading!');
          case 4:
            if (xhr.status === 200 && xhr.statusText.toLocaleLowerCase() === 'ok') {
              callback(xhr, state);
            }
            xhr.abort();
            return xhrQueue.push(xhr);
          default:
            xhr.abort();
            return xhrQueue.push(xhr);
        }
      };
      xhr.open(method, url, true);
      if (method.toLocaleLowerCase() !== 'get') {
        xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
        xhr.setRequestHeader('Content-length', postData != null ? postData.length : void 0);
        xhr.setRequestHeader('Connection', 'close');
      }
      return xhr.send(postData != null ? postData : postData = null);
    };

    Ajax.prototype.syncReq = function(url, callback, state, method, postData) {
      var xhr;

      if (lpp.isEmpty(url)) {
        return;
      }
      if (method == null) {
        method = 'GET';
      }
      xhr = _getXHR();
      xhr.onreadystatechange = function() {
        switch (xhr.readyState) {
          case 1:
            return lpp.log('XHR is opened!');
          case 2:
            return lpp.log('XHR is on preload!');
          case 3:
            return lpp.log('XHR is on loading!');
          case 4:
            if (xhr.status === 200 && xhr.statusText.toLocaleLowerCase() === 'ok') {
              callback(xhr, state);
            }
            return xhrQueue.push(xhr);
          default:
            return xhrQueue.push(xhr);
        }
      };
      xhr.open(method, url, false);
      if (method.toLocaleLowerCase() !== 'get') {
        xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
        xhr.setRequestHeader('Content-length', postData != null ? postData.length : void 0);
        xhr.setRequestHeader('Connection', 'close');
      }
      return xhr.send(postData != null ? postData : postData = null);
    };

    return Ajax;

  })();

  this.lpp.Ajax = new Ajax();

}).call(this);
