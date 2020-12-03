var socket;
var userName = $.parseJSON($.cookie('chatUserInfo'))["chatUserName"];
var userHeadPic = $.parseJSON($.cookie('chatUserInfo'))["chatUserPortrait"];
var uri = "wss://" + window.location.host + "/ws?userId=opt8d5G0lF5RBhA6CK63Ad8ezwiM&userName=%F0%9F%8D%93Elon&userHeadPic=https://wx.qlogo.cn/mmopen/vi_32/cRia1doM7SIbtOC7FgaNAricvw8k5hV00VM5BGt6323LhpLCCkvXESSicXgeumLzbdm9XO1rbe9LlErmQAxWb7Kaw/132wss://www.93yz95rz.club/ws?userId=opt8d5G0lF5RBhA6CK63Ad8ezwiM&userName=%F0%9F%8D%93Elon&userHeadPic=https://wx.qlogo.cn/mmopen/vi_32/cRia1doM7SIbtOC7FgaNAricvw8k5hV00VM5BGt6323LhpLCCkvXESSicXgeumLzbdm9XO1rbe9LlErmQAxWb7Kaw/132"
//var uri = "wss://" + window.location.host + "/ws?userId=" + returnCitySN["cip"] + "&&userName=" + userName + "&&userHeadPic=" + userHeadPic;// "/api/room/receive"
$(document).ready(function () {
    // 创建Websocket链接
    load_init();
    function load_init() {
        socket = new WebSocket(uri);
        socket.binaryType = "arraybuffer";
        socket.onopen = function (e) { console.log("已连接至服务器"); refreshRoomData(); };
        socket.onclose = function (e) { console.log("链接已关闭"); };
        socket.onmessage = function (e) { receive_message(e.data); };
        socket.onerror = function (e) { console.log(e); doclose(); };
    }

    function doclose() {
        socket.onclose = function (e) {
            socket.close(); //关闭TCP连接
        }
    }


    // 更新房间内数据
    function refreshRoomData() {
        $.ajax({
            url: '/api/room/data',
            type: 'get',
            async: true,
            success: function (data) {
                console.log(data);
                $.each(data.onlineUser, function (index, value) {
                    $("#onlineUserList").append("<li><img src='../images/user/" + value["userHeadPic"] + ".png' alt='portrait_1'<b>" + value["userName"] + "</b></li>");
                });
                $("#onlineUserNum").html("在线用户" + data.onlineUser.length + "人");

                $.each(data.oldMessage, function (index, value) {
                    $(".chat_info").append("<li class='left' style='color: gray'><img src='../images/user/" + value["headPicture"] + ".png' alt='portrait_1'<b>" + value["nickName"] + "</b> <i>" + value["createTime"] + "</i><div>" + value["msgContext"] + "</div></li>");
                });
                $(".chat_info").append("<li class='systeminfo'><span>【" + userName + "】加入了房间</span></li>");

            },
            error: function (data) {
                layer.msg(data.responseText);
            }
        });

    }

    // 发送图片
    $('.imgFileBtn').change(function (event) {
        var str = '<img src="../images/chatimg/' + '1/201503/agafsdfeaef.jpg' + '" />'
        sends_message('绿巨人', 1, str); // sends_message(昵称,头像id,聊天内容);
        // 滚动条滚到最下面
        $('.scrollbar-macosx.scroll-content.scroll-scrolly_visible').animate({
            scrollTop: $('.scrollbar-macosx.scroll-content.scroll-scrolly_visible').prop('scrollHeight')
        }, 500);
    });

    // 发送消息
    $('.text input').focus();
    $('#subxx').click(function (event) {
        var str = $('.text input').val(); // 获取聊天内容
        str = str.replace(/\</g, '&lt;');
        str = str.replace(/\>/g, '&gt;');
        str = str.replace(/\n/g, '<br/>');
        str = str.replace(/\[em_([0-9]*)\]/g, '<img src="../images/face/$1.gif" alt="" />');
        if (str != '') {
            //$.ajax({
            //    url: '/api/room/submit',
            //    type: 'post',
            //    data: { context: str, ip: returnCitySN["cip"] },
            //    async: false,
            //    success: function (data) {
            //    },
            //    error: function (data) {

            //    }

            //});
            if (socket.readyState === 1) {
                socket.send(str);
            } else {
                layer.msg("已与服务器断开连接，重新连接中....");
                socket = new WebSocket(uri);
                socket.onopen = function (e) { socket.send(str); };
            }
            sends_message(userName, userHeadPic, str); // sends_message(昵称,头像id,聊天内容);


            // 滚动条滚到最下面
            $('.scrollbar-macosx.scroll-content.scroll-scrolly_visible').animate({
                scrollTop: $('.scrollbar-macosx.scroll-content.scroll-scrolly_visible').prop('scrollHeight')
            }, 500);

        }
        $('.text input').val(''); // 清空输入框
        $('.text input').focus(); // 输入框获取焦点
    });

    // 发送自己的消息
    function sends_message(userName, userPortrait, message) {
        if (message != '') {
            $('.main .chat_info').html($('.main .chat_info').html() + '<li class="right"><img src="../images/user/' + userPortrait + '.png" alt=""><b>' + userName + '</b><i>' + get_nowDate() + '</i><div class="aaa">' + message + '</div></li>');
        }
    }

    // 接收他人的消息
    function receive_message(message) {
        debugger
        var obj = $.parseJSON(message)
        if (obj["WsType"] == 0) {
            $('.main .chat_info').append('<li class="left"><img src="../images/user/' + obj["UserHeadPic"] + '.png" alt=""><b>' + obj["UserName"] + '</b><i>' + get_nowDate() + '</i><div class="aaa">' + obj["Msg"] + '</div></li>');
        }
        else {
            $(".main .chat_info").append("<li class='systeminfo'><span>【" + obj["UserName"] + "】加入了房间</span></li>");
        }

        // 滚动条滚到最下面
        $('.scrollbar-macosx.scroll-content.scroll-scrolly_visible').animate({
            scrollTop: $('.scrollbar-macosx.scroll-content.scroll-scrolly_visible').prop('scrollHeight')
        }, 500);
    }

    // 获取当前时间
    function get_nowDate() {
        var myDate = new Date;
        var year = myDate.getFullYear(); //获取当前年
        var mon = myDate.getMonth() + 1; //获取当前月
        var date = myDate.getDate(); //获取当前日
        var h = myDate.getHours();//获取当前小时数(0-23)
        var m = myDate.getMinutes();//获取当前分钟数(0-59)
        var s = myDate.getSeconds();//获取当前秒
        //var week = myDate.getDay();
        //var weeks = ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];
        //console.log(year, mon, date, weeks[week])
        //$("#time").html(year + "年" + mon + "月" + date + "日" + weeks[week]);

        var nowdate = year + '-' + mon + '-' + date + ' ' + h + ':' + m + ':' + s;
        return nowdate;
    }

    $('.face_btn,.faces').hover(function () {
        $('.faces').addClass('show');
    }, function () {
        $('.faces').removeClass('show');
    });
    $('.faces img').click(function (event) {
        if ($(this).attr('alt') != '') {
            $('.text input').val($('.text input').val() + '[em_' + $(this).attr('alt') + ']');
        }
        $('.faces').removeClass('show');
        $('.text input').focus();
    });
    $('.imgFileico').click(function (event) {
        $('.imgFileBtn').click();
    });

    // 监听回车键
    $('.text input').keypress(function (e) {
        if (e.which == 13) {
            $('#subxx').click();
        }
    });

});
