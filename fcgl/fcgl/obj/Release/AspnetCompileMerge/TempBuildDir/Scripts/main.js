$(document).ready(function ()
{
    $("#btn_file").click(function ()
    {
        $("#file").trigger("click");
    });
    $("#file").change(function ()
    {
        var file = $(this);
        var fileObj = file[0];
        var windowURL = window.URL || window.webkitURL;
        var dataURL;
        if (fileObj && fileObj.files && fileObj.files[0]) {
            dataURL = windowURL.createObjectURL(fileObj.files[0]);
            $("#btn_file").remove();
            $(".div_file").append("<img src='" + dataURL + "' id='btn_file' width='100' />");
            $("#btn_file").click(function () {
                $("#file").trigger("click");
            });
            $("#btn_file").css("margin", "5% 15%");
        }
        else {
            alert("图片获取错误！");
        }
    });
    $("#provinceid").change(function ()
    {
        var provinceid = $(this).val();
        $.post("/HouseProperty/AjaxGetCityList",
            { "provinceid": provinceid },
            function (data)
            {
                $("#cityid").empty();
                data.forEach(function (value, index, arr)
                {
                    $("#cityid").append("<option value='"+value.cityid+"'>"+value.city+"</option>");
                });
                if($("#cityid").css("display")=="none")
                {
                    $("#cityid").css("display", "block");
                }
                $("#cityid").trigger("change");
            }, "json"
        );
    });
    $("#cityid").change(function () {
        var cityid = $(this).val();
        $.post("/HouseProperty/AjaxGetAreaList",
            { "cityid": cityid },
            function (data) {
                $("#areaid").empty();
                data.forEach(function (value, index, arr) {
                    $("#areaid").append("<option value='" + value.id + "'>" + value.area + "</option>");
                });
                if ($("#areaid").css("display") == "none") {
                    $("#areaid").css("display", "inline");
                }
            }, "json"
        );
    });
});
function checkform(that) {
    with (that) {
        $("#adresserr").text("");
        $("#prizeerr").text("");
        $("#detialerr").text("");
        if (adress.value == null || adress.value == "") {
            $("#adresserr").text("地址不能为空！");
            address.focus();
            return false;
        }
        if (prize.value == null || prize.value == "") {
            $("#prizeerr").text("价格不能为空！");
            prize.focus();
            return false;
        }
        else if (isNaN(prize.value)) {
            $("#prizeerr").text("价格必须填数字！");
            prize.focus();
            return false;
        }
        if ($(detial).val() == null || $(detial).val() == "") {
            $("#detialerr").text("详细介绍不能为空！");
            detial.focus();
            return false;
        }
        return true;
    }
}