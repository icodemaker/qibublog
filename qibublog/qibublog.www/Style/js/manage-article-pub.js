!function() {

var editor;
KindEditor.ready(function(K) {
    editor = K.create("#Content", {
        cssPath : "/Content/pub.css",
        bodyClass : "article-content",
        basePath : "/KindEditor/",
        uploadJson : "/Manage_Article/Upload",
        fullscreenShortcut : false,
        items : [
            'formatblock', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', 'strikethrough', 'subscript', 'superscript', '|',
            'link', 'unlink', '|', 'justifyleft', 'justifycenter', 'justifyright', 'justifyfull','indent', 'outdent', '|',
            'insertorderedlist', 'insertunorderedlist', '|', 'image', 'flash', 'insertfile', 'table', 'hr', 'code', '|', 'fullscreen', 'source'
        ]
    });
});


$("form").validate({
    flow: [
        { fields : "Title", errorText : "标题不能为空" },
        { fields : "CategoryId", errorText : "请选择分类" },
        { fields : "Weight", rules : "min:0;max:255", errorText : "权重必须为0-255" },
        { fields : "SummarySize", rules : "min:0;max:9999", errorText : "摘要长度必须为0-9999" },
        { fields : "State", errorText : "请选择文章状态" }
    ]
});

}();