# ±äÁ¿
class VarEl
	constructor: (parentNode, children, ctx, scope, @varName = '') ->
		super(parentNode, children)
		@syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 1, ctx, scope)
		for p, v of @syntaxEl
			@[p] = v if lpp.hasOwnProperty(@syntaxEl, p)

lpp.define 'lpp.tpl.VarEl', VarEl, 'lpp.structure.TreeNode'