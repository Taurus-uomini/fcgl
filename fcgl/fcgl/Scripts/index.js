$(document).ready(function ()
{
    var leng = 0;
    get_start();
    $("#select_search").change(function () {
        var searchs = $("#select_search").val();
        if (searchs == null)
        {
            $("#num").val(0);
            $("#id").val("");
            $("li").remove(".select2-search-choice");
            get_start();
        }
        else
        {
            var searchs = $("#select_search").val();
            leng = $(".select2-search-choice").length;
            $("#num").val(leng);
            $("#id").val(searchs[0]);
            if (leng == 1)
            {
                $.post("/HouseProperty/AjaxGetCityList",
                { "provinceid": searchs[0] },
                function (data) {
                    $("#select_search").empty();
                    data.forEach(function (value, index, arr) {
                        $("#select_search").append("<option class='city' value='" + value.cityid + "'>" + value.city + "</option>");
                    });
                }, "json"
                );
            }
            else if (leng == 2)
            {
                $.post("/HouseProperty/AjaxGetAreaList",
                { "cityid": searchs[0] },
                function (data) {
                    $("#select_search").empty();
                    data.forEach(function (value, index, arr) {
                        $("#select_search").append("<option class='area' value='" + value.areaid + "'>" + value.area + "</option>");
                    });
                }, "json"
                );
            }
            else {
                $("#select_search").empty();
            }
        }   
    });
});
function get_start()
{
    $.post("/HouseProperty/AjaxGetProvinceList",
            function (data) {
                $("#select_search").empty();
                data.forEach(function (value, index, arr) {
                    $("#select_search").append("<option class='province' value='" + value.provinceid + "'>" + value.province + "</option>");
                });
            }, "json"
    );
}