class SectionEl
	constructor: (parentNode, children, ctx, scope, @section) ->
		super(parentNode, children)
		@syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 7, ctx, scope)
		for p, v of @syntaxEl
			@[p] = v if lpp.hasOwnProperty(@syntaxEl, p)


lpp.define 'lpp.tpl.SectionEl', SectionEl