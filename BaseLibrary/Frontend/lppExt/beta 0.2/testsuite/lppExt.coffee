add = (a, b) ->
	a + b
sum = (a) ->
	@s += a 

test 'createCallback', () ->
	equal lppExt.createCallback(add, 1)(2), 3, '1 + 2 = 3'
	equal lppExt.createCallback(add, 2)(4), 6, '2 + 4 = 6'
	equal lppExt.createCallback(add, 2, 5)(4), 7, '2 + 5 = 7'

test 'createDelegate', () ->
	ctx1 = s: 1
	ctx2 = s: 2

	equal lppExt.createDelegate(sum, ctx1)(2), 3, '1 + 2 = 3'
	equal lppExt.createDelegate(sum, ctx2)(2), 4, '2 + 2 = 4'
	#raises lppExt.createDelegate(sum), ReferenceError,'throws exception'