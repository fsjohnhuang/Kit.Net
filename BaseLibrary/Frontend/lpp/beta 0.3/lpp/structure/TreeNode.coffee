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

	### ʵ������ ###
	# ��ӵ�ĳ���ڵ�
	appendTo: (parentNode) ->
		@parentNode = parentNode
		@index = parentNode.children.length
		_recurse(@, _setLevel)
		undefined
	
	# ��ӽڵ㵽��ǰ�ڵ�
	append: (childNode) ->
		@children ?= []
		childNode.index = @children.length
		@children.push(childNode)
		childNode.parentNode = @
		_recurse(childNode, _setLevel)
		undefined

	# ��ȡĳ�ӽڵ�
	getChild: (index) ->
		return null unless (@children?.length > 0) && (@children.length - 1> index)
		return @children[index]

	# ��ȡ��һ���ֵܽڵ�
	getNextSlibing: () ->
		return null unless @parentNode?.children?.length - 1 > @index
		return  @parentNode.children[@index + 1]

	# ��ȡ��һ���ֵܽڵ�
	getPreSlibing: () ->
		return null unless @parentNode? && @index > 0
		return @parentNode.children[@index - 1]

	# ��ȡ���ڵ����һ���ֵܽڵ�
	getNextPSlibing: () ->
		return null unless @parentNode?.parentNode?.children?.length - 1 > @parentNode.index
		return @parentNode.parentNode.children[@parentNode.index + 1]

	# ��ȡ���ڵ����һ���ֵܽڵ�
	getPrePSlibing: () ->
		return null unless @parentNode?.parentNode? && @parentNode.index > 0
		return @parentNode.parentNode.children[@parentNode.index - 1]

	# ˽�з���
	# �ݹ���������ڵ�����
	_recurse = (node, callback) ->
		callback?(node)
		for child in node.children
			_recurse(child, callback)
		undefined

	# ���ýڵ�㼶
	_setLevel = (node) ->
		node.level = node.parentNode.level + 1

lpp.define 'lpp.structure.TreeNode', TreeNode