<template>
    <div class="login-wrap">
        <div class="ms-login">
            <div class="ms-title">投票系统</div>
            <el-form :model="param" ref="login" label-width="0px" class="ms-content">
                <el-form-item prop="username">
                    <el-input v-model="param.username" placeholder="username">
                        <el-button slot="prepend" icon="el-icon-lx-people"></el-button>
                    </el-input>
                </el-form-item>
                <el-form-item prop="password">
                    <el-input
                        type="password"
                        placeholder="password"
                        v-model="param.password"
                        @keyup.enter.native="submitForm()"
                    >
                        <el-button slot="prepend" icon="el-icon-lx-lock"></el-button>
                    </el-input>
                </el-form-item>
                <div class="login-btn">
                    <el-button type="primary" @click="registerVis = true">立即注册</el-button>
                </div>
                <div class="login-btn">
                    <el-button type="primary" @click="submitForm()">登录</el-button>
                </div>
            </el-form>
        </div>

        <!-- 查看弹出框 -->
        <el-dialog title="用户注册" :visible.sync="registerVis" width="30%">
            <el-form ref="form" :model="form" label-width="80px" style="width: 90%;">
                <el-form-item label="用户名">
                    <el-input v-model="form.username"></el-input>
                </el-form-item>
                <el-form-item label="密码">
                    <el-input v-model="form.password" type="password"></el-input>
                </el-form-item>
                <el-form-item label="重复密码">
                    <el-input v-model="repeatPassword" type="password"></el-input>
                </el-form-item>
                <el-form-item label="真实姓名">
                    <el-input v-model="form.name"></el-input>
                </el-form-item>
                <el-form-item label="性别">
                    <el-select v-model="form.sex" placeholder="请选择性别">
                        <el-option label="男" value="男"></el-option>
                        <el-option label="女" value="女"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="年龄">
                    <el-input v-model="form.age"></el-input>
                </el-form-item>
                <el-form-item label="地址">
                    <el-input v-model="form.address"></el-input>
                </el-form-item>
                <el-form-item label="手机号码">
                    <el-input v-model="form.phone"></el-input>
                </el-form-item>
                <el-form-item label="邮箱">
                    <el-input v-model="form.email"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="register()">立即注册</el-button>
                    <el-button @click="registerVis=false">返回</el-button>
                </el-form-item>
            </el-form>
        </el-dialog>
    </div>
</template>

<script>
export default {
    data: function() {
        return {
            param: {
                username: 'admin',
                password: 'admin',
            },
            registerVis: false,
            form: {
                username: '',
                password: '',
                name: '',
                sex: '',
                age: null,
                address: '',
                phone: '',
                email: ''
            },
            repeatPassword: ''
        };
    },
    methods: {
        submitForm() {
            const reg = /^[0-9a-zA-Z]+$/;
            if(this.param.username.length == 0){
                this.$message.error('用户名不能为空');
                return false;
            }
            if(this.param.password.length == 0){
                this.$message.error('密码不能为空');
                return false;
            }
            if(!reg.test(this.param.username)){
                this.$message.error('用户名格式不正确');
                return false;
            }
            if(!reg.test(this.param.password)){
                this.$message.error('密码格式不正确');
                return false;
            }
            var $this = this;
            this.$axios
                .post('/login',this.param)
                .then(function(response) {
                    var data = response.data;
                    console.log(data);
                    if(data.status == 200){
                        $this.$message.success('登录成功');
                        localStorage.setItem('ms_username', $this.param.username);
                        //根据返回回来的数据，判断是否是管理员
                        if(data.data==true){
                            $this.$router.push('/');
                        }else{
                            $this.$router.push('/index');
                        }
                    }else {
                        $this.$message.error('用户名或密码错误');
                    }
                })
                .catch((reason) => this.$message.error('登录失败'))
        },
        register(){
            if(this.form.username.length == 0){
                this.$message.error('用户名不能为空');
                return false;
            }
            if(this.form.password.length == 0){
                this.$message.error('密码不能为空');
                return false;
            }
            if(this.repeatPassword.length == 0){
                this.$message.error('重复密码不能为空');
                return false;
            }
            if(this.form.name.length == 0){
                this.$message.error('真实姓名不能为空');
                return false;
            }
            if(this.form.sex.length == 0){
                this.$message.error('性别不能为空');
                return false;
            }
            if(this.form.age.length == 0){
                this.$message.error('年龄不能为空');
                return false;
            }
            if(this.form.address.length == 0){
                this.$message.error('地址不能为空');
                return false;
            }
            if(this.form.phone.length == 0){
                this.$message.error('手机号不能为空');
                return false;
            }
            if(this.form.email.length == 0){
                this.$message.error('邮箱不能为空');
                return false;
            }
            if(this.repeatPassword != this.form.password){
                this.$message.error("两次输入的密码不相同");
                return false;
            }
            const unameAndPwd = /^[0-9a-zA-Z]+$/;
            if(!unameAndPwd.test(this.form.username)){
                this.$message.error('用户名格式不正确');
                return false;
            }
            if(!unameAndPwd.test(this.form.password)){
                this.$message.error('密码格式不正确');
                return false;
            }
            const nameReg = /^[\u4E00-\u9FA5]+$/;
            if(!nameReg.test(this.form.name)){
                this.$message.error('真实姓名只能存在中文');
                return false;
            }
            const ageReg = /^(?:[1-9][0-9]?|1[01][0-9]|120)$/;
            if(!ageReg.test(this.form.age)){
                this.$message.error('年龄不能为空');
                return false;
            }
            const phoneReg = /^1(3|4|5|6|7|8|9)\d{9}$/;
            if(!phoneReg.test(this.form.phone)){
                this.$message.error('手机号格式不正确');
                return false;
            }
            const emailReg = /^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/;
            if(!emailReg.test(this.form.email)){
                this.$message.error('邮箱格式不正确');
                return false;
            }
            this.form.age = Number(this.form.age);
            this.$axios.post('/user/register',this.form)
                .then((response) => {
                    var data = response.data;
                    if(data.status == 200) {
                        this.$message.success('注册成功');
                        this.registerVis = false;
                    }else{
                        this.$message.error('注册失败，请稍后重试');
                    }
                })
                .catch((reason) => this.$message.error("注册失败"))
        }
    },
};
</script>

<style scoped>
.login-wrap {
    position: relative;
    width: 100%;
    height: 100%;
    background-image: url(../../assets/img/login-bg.jpg);
    background-size: 100%;
}
.ms-title {
    width: 100%;
    line-height: 50px;
    text-align: center;
    font-size: 20px;
    color: #fff;
    border-bottom: 1px solid #ddd;
}
.ms-login {
    position: absolute;
    left: 50%;
    top: 50%;
    width: 350px;
    margin: -190px 0 0 -175px;
    border-radius: 5px;
    background: rgba(255, 255, 255, 0.3);
    overflow: hidden;
}
.ms-content {
    padding: 30px 30px;
}
.login-btn {
    text-align: center;
}
.login-btn button {
    width: 100%;
    height: 36px;
    margin-bottom: 10px;
}
.login-tips {
    font-size: 12px;
    line-height: 30px;
    color: #fff;
}
</style>
