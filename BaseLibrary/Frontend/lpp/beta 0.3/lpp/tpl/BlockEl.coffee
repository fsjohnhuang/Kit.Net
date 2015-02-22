class BlockEl
	constructor: (parentNode, children, ctx, scope) ->
		super(parentNode, children)
		@syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 6, ctx, scope)
		for p, v of @syntaxEl
			@[p] = v if lpp.hasOwnProperty(@syntaxEl, p)


lpp.define 'lpp.tpl.BlockEl', BlockEl, 'lpp.structure.TreeNode'