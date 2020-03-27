var userName;
$(document).ready(function () {
    var userInfo = {};
    var userCookie = $.cookie('chatUserInfo');
    var requestHref = window.location.toString();    
    var isHaveCookies = userCookie == undefined || userCookie.lenth == 0 ? false : true;
    if (requestHref.indexOf("login.html") == -1) {
        if (!isHaveCookies) {
            layer.msg('用户信息已过期，请滚粗~', function () {
                window.location.href = 'login.html';
            });
        }
        else {
            if (requestHref.indexOf("index.html") > 0) {
                setUserInfo();
            }
        }
    }
    else {
        if (isHaveCookies) {
            window.location.href = 'index.html';
        }
    }

    // 设置index用户信息
    function setUserInfo() {
        setUserName($.parseJSON(userCookie)["chatUserName"]);
        setUserPic($.parseJSON(userCookie)["chatUserPortrait"])
        var theme_id = isHaveCookies ? $.cookie('chatUserTheme') : 1;
        $('body').css('background-image', 'url(../images/theme/' + theme_id + '_bg.jpg)');
    }

    // -------------------------登录页面---------------------------------------------------

    // 登录按钮
    $('#login').click(function (event) {

        var userName = $('.login input').val(); // 用户昵称
        var userPortrait = $('.login img').attr('portrait_id'); // 用户头像id
        if (userName == '') { // 如果不填昵称就给 "User" + ID
            userName = '用户-' + Date.parse(new Date());
        }

        userInfo.chatUserName = userName;
        userInfo.chatUserPortrait = userPortrait;
        userInfo.chatUserIp = returnCitySN["cip"];

        $.cookie('chatUserInfo', JSON.stringify(userInfo), { expires: 7 });  // { expires: 7, path: '/' }有效路径
        window.location.href = 'index.html'; // 页面跳转
    });

    // ------------------------选择聊天室页面-----------------------------------------------

    // 用户信息提交
    $('#userinfo_sub').click(function (event) {
        var userName = $('.rooms .user_name input').val(); // 用户昵称
        var userPortrait = $('.rooms .user_portrait img').attr('portrait_id'); // 用户头像id
        if (userName == '') { // 如果不填用户昵称，就是以前的昵称
            userName = $('.rooms .user_name input').attr('placeholder');
        }

        userInfo.chatUserName = userName;
        userInfo.chatUserPortrait = userPortrait;
        $.cookie('chatUserInfo', JSON.stringify(userInfo), { expires: 7 });
        setUserName(userName);
        setUserPic(userPortrait);
    });

    // 设置用户名称
    function setUserName(userName) {
        $('.userinfo a b').text(userName); // 修改标题栏的用户昵称
        $('.rooms .user_name input').val(''); // 昵称输入框清空
        $('.rooms .user_name input').attr('placeholder', userName); // 昵称输入框默认显示用户昵称
        $('.topnavlist .popover').not($(this).next('.popover')).removeClass('show'); // 关掉用户面板
        $('.clapboard').addClass('hidden'); // 关掉模糊背景
    }

    // 设置头像
    function setUserPic(portrait_id) {
        $('.select_portrait img').removeClass('t');
        $('.select_portrait img[portrait_id =' + portrait_id + '] ').addClass('t');
        $('.user_portrait img').attr('portrait_id', portrait_id);
        $('.user_portrait img').attr('src', '../images/user/' + portrait_id + '.png');
        //var userPortrait = $('.rooms .user_portrait img').attr(portrait_id); // 用户头像id
    }

    // 设置主题
    $('.theme img').click(function (event) {
        var theme_id = $(this).attr('theme_id');
        $('.clapboard').click(); // 关掉用户模糊背景
        $.cookie('chatUserTheme', theme_id, { expires: 7 });
        $('body').css('background-image', 'url(../images/theme/' + theme_id + '_bg.jpg)'); // 设置背景
    });


    // -----下边的代码不用管---------------------------------------

    jQuery('.scrollbar-macosx').scrollbar();
    $('.topnavlist li a').click(function (event) {
        $('.topnavlist .popover').not($(this).next('.popover')).removeClass('show');
        $(this).next('.popover').toggleClass('show');
        if ($(this).next('.popover').attr('class') != 'popover fade bottom in') {
            $('.clapboard').removeClass('hidden');
        } else {
            $('.clapboard').click();
        }
    });
    $('.clapboard').click(function (event) {
        $('.topnavlist .popover').removeClass('show');
        $(this).addClass('hidden');
        //$('.user_portrait img').attr('portrait_id', $('.user_portrait img').attr('ptimg'));
        //$('.user_portrait img').attr('src', '../images/user/' + $('.user_portrait img').attr('ptimg') + '.png');
        //$('.select_portrait img').removeClass('t');
        //$('.select_portrait img').eq($('.user_portrait img').attr('ptimg') - 1).addClass('t');
        //$('.rooms .user_name input').val('');
    });
    $('.select_portrait img').hover(function () {
        var portrait_id = $(this).attr('portrait_id');
        $('.user_portrait img').attr('src', '../images/user/' + portrait_id + '.png');
    }, function () {
        var t_id = $('.user_portrait img').attr('portrait_id');
        $('.user_portrait img').attr('src', '../images/user/' + t_id + '.png');
    });
    $('.select_portrait img').click(function (event) {
        var portrait_id = $(this).attr('portrait_id');
        $('.user_portrait img').attr('portrait_id', portrait_id);
        $('.select_portrait img').removeClass('t');
        $(this).addClass('t');
    });

    // 监听回车键
    $(document).keyup(function (event) {
        if (event.keyCode == 13) {
            $("#login").trigger("click");
        }
    });

});
