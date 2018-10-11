(function (window) {
    var Change = {
        iniBtn: function (btnEles) {
            for (var i = 0; i < btnEles.length; i++) {
                btnEles[i].onmouseover = function () {
                    this.style.backgroundPositionY = "-48px";
                }
                btnEles[i].onmousedown = function (e) {
                    if (e.button == 0) {
                        this.style.backgroundPositionY = "-96px";
                    }
                }
                btnEles[i].onmouseup = function () {
                    this.style.backgroundPositionY = "-48px";
                    if (this.getAttribute("id") == "btnReset") {
                        document.getElementById("mmLv").style.opacity = "0";
                        var inputs = document.getElementsByTagName("input");
                        for (var i = 0; i < inputs.length - 1; i++) {
                            inputs[i].value = "";
                            inputs[i].nextSibling.style.opacity = "0";
                            inputs[i].nextSibling.nextSibling.style.opacity = "0";
                        }
                    } else if (this.getAttribute("id") == "btnReg") {
                        var regId = document.getElementById("txtRegId");
                        var pwd1 = document.getElementById("txtRegPwd");
                        var hiddenReg = document.getElementById("hiddenRegPwd");
                        var pwd2 = document.getElementById("txtRepeatRegPwd");
                        var ifyCode = document.getElementById("txtCheckImg");
                        var rgxSpe = /[^a-zA-Z0-9\-_]+/;
                        if (regId.value == "" || pwd1.value == "" || pwd2.value == "" || ifyCode.value == "") {
                            alert("注册信息填写不完整");
                        } else if (pwd1.value != pwd2.value) {
                            alert("两次输入密码不一致");
                        } else if (rgxSpe.test(regId.value) || rgxSpe.test(pwd1.value)) {
                            alert("登录名或密码中包含非法字符，不能包含除字母、数字、_、-以外的字符");
                        } else if (regId.value.length < 6 || regId.value.length > 16 || pwd1.value.length < 6 || pwd1.value.length > 16) {
                            alert("登录名或密码长度有误，应为6-16");
                        } else {
                            hiddenReg.value = hex_md5(pwd1.value);
                            var xhr = null;
                            if (XMLHttpRequest) {
                                xhr = new XMLHttpRequest();
                            } else if (ActiveXObject) {
                                xhr = new ActiveXObject("Microsoft.XMLHttp");
                            }
                            xhr.onreadystatechange = function () {
                                if (xhr.readyState == 4 && xhr.status == 200) {
                                    if (xhr.responseText == "ok") {
                                        window.location.href = "/RegJump.html";
                                    } else{
                                        alert("注册失败。\n可能的原因：\n1.登录名已存在；\n2.两次输入密码不一致；\n3.登录名或密码中包含非法字符或长度有误；\n4.验证码不正确；\n5.注册信息填写不完整");
                                    } 
                                }
                            }
                            xhr.open("post", "/Register/UserReg", true);
                            xhr.setRequestHeader("content-type", "application/x-www-form-urlencoded");
                            xhr.send("regId=" + regId.value + "&regPwd=" + hiddenReg.value + "&ifyCode=" + ifyCode.value);
                        }
                    }
                }
                btnEles[i].onmouseleave = function () {
                    this.style.backgroundPositionY = "0";
                }
            }
        },
        tipsChange: function (txts) {
            var rgxSpe = /[^a-zA-Z0-9\-_]+/;
            for (var i = 0; i < txts.length - 1; i++) {
                txts[i].onfocus = function () {
                    this.nextSibling.style.opacity = "0";
                    this.nextSibling.nextSibling.style.opacity = "1";
                    this.nextSibling.nextSibling.innerHTML = this.id == "txtCheckImg" ? "点击图片更换验证码(不区分大小写)" : this.id == "txtRepeatRegPwd" ? "再次输入密码" : "6-16位字母、数字、下划线(_)、连字符(-)组合";
                    this.nextSibling.nextSibling.style.color = "#333";
                }
                txts[i].onblur = function () {
                    if (this.value == "") {
                        this.nextSibling.style.backgroundPositionX = "-130px";
                        this.nextSibling.innerHTML = "*";
                        this.nextSibling.style.opacity = "1";
                        this.nextSibling.nextSibling.innerHTML = this.id == "txtRegId" ? "登录名不能为空" : this.id == "txtCheckImg" ? "验证码不能为空" : "密码不能为空";
                        this.nextSibling.nextSibling.style.color = "red";
                    } else if (this.id != "txtCheckImg" && (rgxSpe.test(this.value) || this.value.length < 6 || this.value.length > 16)) {
                        this.nextSibling.style.backgroundPosition = "-109px -86px";
                        this.nextSibling.innerHTML = " ";
                        this.nextSibling.style.opacity = "1";
                        this.nextSibling.nextSibling.innerHTML = this.id == "txtRegId" ? this.value.length < 6 ? "登录名过短" : "登录名过长" : this.value.length < 6 ? "密码过短" : "密码过长";
                        this.nextSibling.nextSibling.style.color = "red";
                    } else if (this.id == "txtRegId") {
                        this.nextSibling.nextSibling.style.opacity = "0";
                        initjs.doAjax(this, "/Register/CheckInput", "regId=" + this.value);
                    } else if (this.id == "txtCheckImg") {
                        this.nextSibling.nextSibling.style.opacity = "0";
                        initjs.doAjax(this, "/Register/CheckInput", "ifyCode=" + this.value);
                    } else if (this.id == "txtRepeatRegPwd" && this.value != txts[1].value) {
                        this.nextSibling.style.backgroundPosition = "-109px -86px";
                        this.nextSibling.innerHTML = " ";
                        this.nextSibling.style.opacity = "1";
                        this.nextSibling.nextSibling.innerHTML = "两次密码不一致";
                        this.nextSibling.nextSibling.style.color = "red";
                    } else {
                        this.nextSibling.style.backgroundPosition = "-109px -38px";
                        this.nextSibling.innerHTML = " ";
                        this.nextSibling.style.opacity = "1";
                        this.nextSibling.nextSibling.style.opacity = "0";
                    }

                }
                txts[i].onkeyup = function () {
                    if (this.id == "txtRegPwd") {
                        var mmLv = document.getElementById("mmLv");
                        var mmLevel = 0;
                        var mmInput = this.value;
                        if (mmInput != "") {
                            if (/[a-z]+/.test(mmInput)) {
                                mmLevel++;
                            }
                            if (/[A-Z]+/.test(mmInput)) {
                                mmLevel++;
                            }
                            if (/\d+/.test(mmInput)) {
                                mmLevel++;
                            }
                            if (/[\-_]+/.test(mmInput)) {
                                mmLevel++;
                            }
                            if (mmInput.length < 8) {
                                mmLevel--;
                            }
                            mmLevel = mmLevel < 0 ? 0 : mmLevel;
                            mmLv.style.opacity = "1";
                            mmLv.style.backgroundPositionY = -10 * mmLevel + "px";
                        } else {
                            mmLv.style.opacity = "0";
                        }
                    }
                }
            }
        },
        doAjax: function (txtInput, url, pString) {
            var xhr = XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    if (xhr.responseText == "ok") {
                        txtInput.nextSibling.style.backgroundPosition = "-109px -38px";
                        txtInput.nextSibling.innerHTML = " ";
                        txtInput.nextSibling.style.opacity = "1";
                        txtInput.nextSibling.nextSibling.style.opacity = "0";
                    } else {
                        txtInput.nextSibling.style.backgroundPosition = "-109px -86px";
                        txtInput.nextSibling.innerHTML = " ";
                        txtInput.nextSibling.style.opacity = "1";
                        txtInput.nextSibling.nextSibling.innerHTML = txtInput.id == "txtRegId" ? "登录名已存在" : "验证码不正确";
                        txtInput.nextSibling.nextSibling.style.opacity = "1";
                        txtInput.nextSibling.nextSibling.style.color = "red";
                    }
                }
            };
            xhr.open("post", url, true);
            xhr.setRequestHeader("content-Type", "application/x-www-form-urlencoded");
            xhr.send(pString);
        },
        addLoadEvent: function (func) {
            var oldOnload = window.onload;
            if (typeof (window.onload) != "function") {
                window.onload = func;
            } else {
                window.onload = function () {
                    oldOnload();
                    func();
                }
            }
        }
    };
    window.initjs = Change;
})(window);