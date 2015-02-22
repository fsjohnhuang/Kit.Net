# Ñ­»·Óï¾ä
class LoopEl
	constructor: (parentNode, children, ctx, scope, @condition) ->
		super(parentNode, children)
		@syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 4, ctx, scope)
		for p, v of @syntaxEl
			@[p] = v if lpp.hasOwnProperty(@syntaxEl, p)


lpp.define 'lpp.tpl.LoopEl', LoopEl, 'lpp.structure.TreeNode'