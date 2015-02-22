(function() {
  test('test add with 1,2', function() {
    var proxy;

    proxy = this.spy(add);
    proxy(1, 2);
    proxy(2, 3);
    return ok(proxy.calledOnce, 'add called once.');
  });

}).call(this);
