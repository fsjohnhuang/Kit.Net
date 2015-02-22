class TreeNode

	constructor: (@parentNode = null, @children = []) ->
		@level = if parentNode?.level? then parentNode.level + 1 else 0
		@index = if parentNode?.children? then parentNode.children.length else 0
		parentNode?.children?.push(@)
		if lpp.isArray(children)
			for child, i in children
				child.parentNode = @
				child.index = i
				_recurse(child, _setLevel)

	### 实例方法 ###
	# 添加到某父节点
	appendTo: (parentNode) ->
		@parentNode = parentNode
		@index = parentNode.children.length
		_recurse(@, _setLevel)
		undefined
	
	# 添加节点到当前节点
	append: (childNode) ->
		@children ?= []
		childNode.index = @children.length
		@children.push(childNode)
		childNode.parentNode = @
		_recurse(childNode, _setLevel)
		undefined

	# 获取某子节点
	getChild: (index) ->
		return null unless (@children?.length > 0) && (@children.length - 1> index)
		return @children[index]

	# 获取下一个兄弟节点
	getNextSlibing: () ->
		return null unless @parentNode?.children?.length - 1 > @index
		return  @parentNode.children[@index + 1]

	# 获取上一个兄弟节点
	getPreSlibing: () ->
		return null unless @parentNode? && @index > 0
		return @parentNode.children[@index - 1]

	# 获取父节点的下一个兄弟节点
	getNextPSlibing: () ->
		return null unless @parentNode?.parentNode?.children?.length - 1 > @parentNode.index
		return @parentNode.parentNode.children[@parentNode.index + 1]

	# 获取父节点的上一个兄弟节点
	getPrePSlibing: () ->
		return null unless @parentNode?.parentNode? && @parentNode.index > 0
		return @parentNode.parentNode.children[@parentNode.index - 1]

	# 私有方法
	# 递归设置子孙节点属性
	_recurse = (node, callback) ->
		callback?(node)
		for child in node.children
			_recurse(child, callback)
		undefined

	# 设置节点层级
	_setLevel = (node) ->
		node.level = node.parentNode.level + 1

lpp.define 'lpp.structure.TreeNode', TreeNode