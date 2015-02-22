config = 
	extend: 'Ext.form.Panel'
	alias: 'widget.lppUploadPanel'
	uploadUrl: ''
	afterUpload: ->
	initComponent: ->
		if @isSingle
			@tbar = [{
				icon: "#{lppExt.util.Resource.IMG}arrow_up.png"
				text: '上传'
				handler: @upload_click
				scope: @
			}]
		else	
			@tbar = [{
				icon: "#{lppExt.util.Resource.IMG}add.png"
				text: '添加'
				handler: @add_click
				scope: @
			},{
				icon: "#{lppExt.util.Resource.IMG}arrow_up.png"
				text: '上传'
				handler: @upload_click
				scope: @
			}]

		@items = [] if not @items?
		@items.push
			xtype: 'lppUploadField'
			name: 'file0'
			deletable: false

		@callParent arguments
		undefined
	add_click: ->
		now = new Date()
		now = now.getMilliseconds() + now.getSeconds() 
		@add
			xtype: 'lppUploadField'
			name: 'file' + now
			host: @
		undefined
	upload_click: ->
		basicFrm = @getForm()
		if basicFrm.isValid()
			basicFrm.submit
				clientVaildation: true
				waitTitle: lppExt.util.Msg.TIPS_TITLE
				waitMsg: lppExt.util.Msg.SUBMITING_MSG
				url: @uploadUrl
				scope: @
				success: (form, action) ->
					result = action.result
					uploadFields = []
					for f, i in @items
						uploadFields.push @items.items[i] if @items.items[i].xtype is 'lppUploadField'
					@remove field for field, i in uploadFields when i > 0
					@afterUpload? result
					undefined
				failure: (form, action) ->
					switch action.failureType 
						when Ext.form.action.Action.CONNECT_FAILURE
							Ext.Msg.alert lppExt.util.Msg.TIPS_TITLE, lppExt.util.Msg.CONNECTION_ERROR
						when Ext.form.action.Action.SERVER_INVALID
							Ext.Msg.alert lppExt.util.Msg.TIPS_TITLE, action.result.msg
		undefined

Ext.syncRequire 'lppExt.upload.lppUploadField'
Ext.define  'lppExt.upload.lppUploadPanel', config