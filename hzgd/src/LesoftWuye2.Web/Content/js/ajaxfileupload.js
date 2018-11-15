
//jQuery.extend({
	

//    createUploadIframe: function(id, uri)
//	{
//			//create frame
//            var frameId = 'jUploadFrame' + id;
//            var iframeHtml = '<iframe id="' + frameId + '" name="' + frameId + '" style="position:absolute; top:-9999px; left:-9999px"';
//			if(window.ActiveXObject)
//			{
//                if(typeof uri== 'boolean'){
//					iframeHtml += ' src="' + 'javascript:false' + '"';

//                }
//                else if(typeof uri== 'string'){
//					iframeHtml += ' src="' + uri + '"';

//                }	
//			}
//			iframeHtml += ' />';
//			jQuery(iframeHtml).appendTo(document.body);

//            return jQuery('#' + frameId).get(0);			
//    },
//    createUploadForm: function(id, fileElementId, data)
//	{
//		//create form	
//		var formId = 'jUploadForm' + id;
//		var fileId = 'jUploadFile' + id;
//		var form = jQuery('<form  action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data"></form>');	
//		if(data)
//		{
//			for(var i in data)
//			{
//				jQuery('<input type="hidden" name="' + i + '" value="' + data[i] + '" />').appendTo(form);
//			}			
//		}		
//		var oldElement = jQuery('#' + fileElementId);
//		var newElement = jQuery(oldElement).clone();
//		jQuery(oldElement).attr('id', fileId);
//		jQuery(oldElement).before(newElement);
//		jQuery(oldElement).appendTo(form);


		
//		//set attributes
//		jQuery(form).css('position', 'absolute');
//		jQuery(form).css('top', '-1200px');
//		jQuery(form).css('left', '-1200px');
//		jQuery(form).appendTo('body');		
//		return form;
//    },

//    ajaxFileUpload: function(s) {
//        // TODO introduce global settings, allowing the client to modify them for all requests, not only timeout		
//        s = jQuery.extend({}, jQuery.ajaxSettings, s);
//        var id = new Date().getTime()        
//		var form = jQuery.createUploadForm(id, s.fileElementId, (typeof(s.data)=='undefined'?false:s.data));
//		var io = jQuery.createUploadIframe(id, s.secureuri);
//		var frameId = 'jUploadFrame' + id;
//		var formId = 'jUploadForm' + id;		
//        // Watch for a new set of requests
//        if ( s.global && ! jQuery.active++ )
//		{
//			jQuery.event.trigger( "ajaxStart" );
//		}            
//        var requestDone = false;
//        // Create the request object
//        var xml = {}   
//        if ( s.global )
//            jQuery.event.trigger("ajaxSend", [xml, s]);
//        // Wait for a response to come back
//        var uploadCallback = function(isTimeout)
//		{			
//			var io = document.getElementById(frameId);
//            try 
//			{				
//				if(io.contentWindow)
//				{
//					 xml.responseText = io.contentWindow.document.body?io.contentWindow.document.body.innerHTML:null;
//                	 xml.responseXML = io.contentWindow.document.XMLDocument?io.contentWindow.document.XMLDocument:io.contentWindow.document;
					 
//				}else if(io.contentDocument)
//				{
//					 xml.responseText = io.contentDocument.document.body?io.contentDocument.document.body.innerHTML:null;
//                	xml.responseXML = io.contentDocument.document.XMLDocument?io.contentDocument.document.XMLDocument:io.contentDocument.document;
//				}						
//            }catch(e)
//			{
//				jQuery.handleError(s, xml, null, e);
//			}
//            if ( xml || isTimeout == "timeout") 
//			{				
//                requestDone = true;
//                var status;
//                try {
//                    status = isTimeout != "timeout" ? "success" : "error";
//                    // Make sure that the request was successful or notmodified
//                    if ( status != "error" )
//					{
//                        // process the data (runs the xml through httpData regardless of callback)
//                        var data = jQuery.uploadHttpData( xml, s.dataType );    
//                        // If a local callback was specified, fire it and pass it the data
//                        if ( s.success )
//                            s.success( data, status );
    
//                        // Fire the global callback
//                        if( s.global )
//                            jQuery.event.trigger( "ajaxSuccess", [xml, s] );
//                    } else
//                        jQuery.handleError(s, xml, status);
//                } catch(e) 
//				{
//                    status = "error";
//                    jQuery.handleError(s, xml, status, e);
//                }

//                // The request was completed
//                if( s.global )
//                    jQuery.event.trigger( "ajaxComplete", [xml, s] );

//                // Handle the global AJAX counter
//                if ( s.global && ! --jQuery.active )
//                    jQuery.event.trigger( "ajaxStop" );

//                // Process result
//                if ( s.complete )
//                    s.complete(xml, status);

//                jQuery(io).unbind()

//                setTimeout(function()
//									{	try 
//										{
//											jQuery(io).remove();
//											jQuery(form).remove();	
											
//										} catch(e) 
//										{
//											jQuery.handleError(s, xml, null, e);
//										}									

//									}, 100)

//                xml = null

//            }
//        }
//        // Timeout checker
//        if ( s.timeout > 0 ) 
//		{
//            setTimeout(function(){
//                // Check to see if the request is still happening
//                if( !requestDone ) uploadCallback( "timeout" );
//            }, s.timeout);
//        }
//        try 
//		{

//			var form = jQuery('#' + formId);
//			jQuery(form).attr('action', s.url);
//			jQuery(form).attr('method', 'POST');
//			jQuery(form).attr('target', frameId);
//            if(form.encoding)
//			{
//				jQuery(form).attr('encoding', 'multipart/form-data');      			
//            }
//            else
//			{	
//				jQuery(form).attr('enctype', 'multipart/form-data');			
//            }			
//            jQuery(form).submit();

//        } catch(e) 
//		{			
//            jQuery.handleError(s, xml, null, e);
//        }
		
//		jQuery('#' + frameId).load(uploadCallback	);
//        return {abort: function () {}};	

//    },

//    uploadHttpData: function( r, type ) {
//        var data = !type;
//        data = type == "xml" || data ? r.responseXML : r.responseText;
//        // If the type is "script", eval it in global context
//        if ( type == "script" )
//            jQuery.globalEval( data );
//        // Get the JavaScript object, if JSON is used.
//        if ( type == "json" )
//            eval( "data = " + data );
//        // evaluate scripts within html
//        if ( type == "html" )
//            jQuery("<div>").html(data).evalScripts();

//        return data;
//    }
//})


/*
// jQuery Ajax File Uploader
//
// @author: Jordan Feldstein <jfeldstein.com>
//
//  - Ajaxifies an individual <input type="file">
//  - Files are sandboxed. Doesn't matter how many, or where they are, on the page.
//  - Allows for extra parameters to be included with the file
//  - onStart callback can cancel the upload by returning false
*/


(function ($) {
    $.fn.ajaxfileupload = function (options) {
        var settings = {
            params: {},
            action: '',
            onStart: function () { },
            onComplete: function (response) { },
            onCancel: function () { },
            validate_extensions: true,
            valid_extensions: ['gif', 'png', 'jpg', 'jpeg'],
            submit_button: null
        };

        var uploading_file = false;

        if (options) {
            $.extend(settings, options);
        }


        // 'this' is a jQuery collection of one or more (hopefully) 
        //  file elements, but doesn't check for this yet
        return this.each(function () {
            var $element = $(this);

            // Skip elements that are already setup. May replace this 
            //  with uninit() later, to allow updating that settings
            if ($element.data('ajaxUploader-setup') === true) return;

            $element.change(function () {
                // since a new image was selected, reset the marker
                uploading_file = false;

                // only update the file from here if we haven't assigned a submit button
                if (settings.submit_button == null) {
                    upload_file();
                }
            });

            if (settings.submit_button == null) {
                // do nothing
            } else {
                settings.submit_button.click(function (e) {
                    // Prevent non-AJAXy submit
                    e.preventDefault();

                    // only attempt to upload file if we're not uploading
                    if (!uploading_file) {
                        upload_file();
                    }
                });
            }

            var upload_file = function () {
                if ($element.val() == '') return settings.onCancel.apply($element, [settings.params]);

                // make sure extension is valid
                var ext = $element.val().split('.').pop().toLowerCase();
                if (true === settings.validate_extensions && $.inArray(ext, settings.valid_extensions) == -1) {
                    // Pass back to the user
                    settings.onComplete.apply($element, [{ status: false, message: 'The select file type is invalid. File must be ' + settings.valid_extensions.join(', ') + '.' }, settings.params]);
                } else {
                    uploading_file = true;

                    // Creates the form, extra inputs and iframe used to 
                    //  submit / upload the file
                    wrapElement($element);

                    // Call user-supplied (or default) onStart(), setting
                    //  it's this context to the file DOM element
                    var ret = settings.onStart.apply($element, [settings.params]);

                    // let onStart have the option to cancel the upload
                    if (ret !== false) {
                        $element.parent('form').submit(function (e) { e.stopPropagation(); }).submit();
                    } else {
                        uploading_file = false;
                    }
                }
            };

            // Mark this element as setup
            $element.data('ajaxUploader-setup', true);

            /*
            // Internal handler that tries to parse the response 
            //  and clean up after ourselves. 
            */
            var handleResponse = function (loadedFrame, element) {
                var response, responseStr = $(loadedFrame).contents().text();
                try {
                    //response = $.parseJSON($.trim(responseStr));
                    response = JSON.parse(responseStr);
                } catch (e) {
                    response = responseStr;
                }

                // Tear-down the wrapper form
                element.siblings().remove();
                element.unwrap();

                uploading_file = false;

                // Pass back to the user
                settings.onComplete.apply(element, [response, settings.params]);
            };

            /*
            // Wraps element in a <form> tag, and inserts hidden inputs for each
            //  key:value pair in settings.params so they can be sent along with
            //  the upload. Then, creates an iframe that the whole thing is 
            //  uploaded through. 
            */
            var wrapElement = function (element) {
                // Create an iframe to submit through, using a semi-unique ID
                var frame_id = 'ajaxUploader-iframe-' + Math.round(new Date().getTime() / 1000)
                $('body').after('<iframe width="0" height="0" style="display:none;" name="' + frame_id + '" id="' + frame_id + '"/>');
                $('#' + frame_id).get(0).onload = function () {
                    handleResponse(this, element);
                };

                // Wrap it in a form
                element.wrap(function () {
                    return '<form action="' + settings.action + '" method="POST" enctype="multipart/form-data" target="' + frame_id + '" />'
                })
                // Insert <input type='hidden'>'s for each param
                .before(function () {
                    var key, html = '';
                    for (key in settings.params) {
                        var paramVal = settings.params[key];
                        if (typeof paramVal === 'function') {
                            paramVal = paramVal();
                        }
                        html += '<input type="hidden" name="' + key + '" value="' + paramVal + '" />';
                    }
                    return html;
                });
            }
        });
    }
})(jQuery)