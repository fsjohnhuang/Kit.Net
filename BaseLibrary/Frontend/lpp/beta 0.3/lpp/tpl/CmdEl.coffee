class CmdEl
	constructor: (parentNode, children, ctx, scope, @cmd) ->
		super(parentNode, children)
		@syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 5, ctx, scope)
		for p, v of @syntaxEl
			@[p] = v if lpp.hasOwnProperty(@syntaxEl, p)


lpp.define 'lpp.tpl.CmdEl', CmdEl, 'lpp.structure.TreeNode'