# ÆÕÍ¨ÎÄ±¾
class StrEl
	constructor: (parentNode, children, ctx, scope, @content = '') ->
		super(parentNode, children)
		@syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 0, ctx, scope)
		for p, v of @syntaxEl
			@[p] = v if lpp.hasOwnProperty(@syntaxEl, p)

lpp.define 'lpp.tpl.StrEl', StrEl, 'lpp.structure.TreeNode'