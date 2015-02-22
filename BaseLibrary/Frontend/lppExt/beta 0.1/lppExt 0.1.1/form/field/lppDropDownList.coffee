config = 
	extend: 'Ext.form.field.ComboBox'
	alias: 'widget.lppDropDownList'
	queryMode: 'local'
	allowBlank: false
	editable: false
	url: ''
	fields: []
	initComponent: ->
		#获取数据
		if @url? and @url isnt ''
			lppExt.util.Common.syncRequest 
				url: @url
				scope: @
				success: (response, eOpt) ->
					jsonResult = Ext.decode response.responseText
					if not jsonResult.success then return

					@store = 
						fields: @fields
						data: jsonResult.items

		@callParent arguments
		undefined

Ext.define 'lppExt.form.field.lppDropDownList', config