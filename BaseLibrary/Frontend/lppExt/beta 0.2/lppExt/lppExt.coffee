Ext.define 'lppExt', {
	statics: 
		createDelegate: (fn, ctx = window, addition...) ->
			(args...) ->
				fn.apply ctx, addition.concat(args)
		createCallback: (fn, addition...) ->
			(args...) ->
				fn.apply window, addition.concat(args)
}