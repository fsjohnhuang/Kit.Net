class IfEl
	constructor: (parentNode, children, ctx, scope, @condition) ->
		super(parentNode, children)
		@syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 3, ctx, scope)
		for p, v of @syntaxEl
			@[p] = v if lpp.hasOwnProperty(@syntaxEl, p)


lpp.define 'lpp.tpl.IfEl', IfEl, 'lpp.structure.TreeNode'