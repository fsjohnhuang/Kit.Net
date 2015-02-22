test 'test add with 1,2', ->
	proxy = @spy(add)
	proxy 1, 2
	proxy 2, 3
	ok proxy.calledOnce, 'add called once.'