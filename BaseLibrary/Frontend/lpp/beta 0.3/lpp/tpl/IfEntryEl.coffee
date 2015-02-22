# 条件语句入口节点
class IfEntryEl
	constructor: (parentNode, children, ctx, scope) ->
		super(parentNode, children)
		@syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 3, ctx, scope)
		for p, v of @syntaxEl
			@[p] = v if lpp.hasOwnProperty(@syntaxEl, p)


lpp.define 'lpp.tpl.IfEntryEl', IfEntryEl, 'lpp.structure.TreeNode'