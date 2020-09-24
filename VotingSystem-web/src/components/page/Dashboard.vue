<template>
    <div>
        <el-row :gutter="20">
            <el-col :span="8">
                <el-card shadow="hover" class="mgb20" style="height:252px;">
                    <div class="user-info">
                        <img src="../../assets/img/img.jpg" class="user-avator" alt/>
                        <div class="user-info-cont">
                            <div>管理员</div>
                        </div>
                    </div>
                    <div class="user-info-list">
                        上次登录时间：
                        <span>2020-09-09</span>
                    </div>
                    <div class="user-info-list">
                        上次登录地点：
                        <span>成都</span>
                    </div>
                </el-card>
                <el-card shadow="hover" style="height:252px;">
                    <div slot="header" class="clearfix">
                        <span>最新投票时间进度</span>
                    </div>
                    <div v-for="(vote) in votes" :key="vote.id">
                        {{vote.title}}
                        <el-progress :percentage="getSchedule(vote)" color="#42b983"></el-progress>
                    </div>

                    <!-- JavaScript
                    <el-progress :percentage="24.1" color="#f1e05a"></el-progress>
                    CSS
                    <el-progress :percentage="13.7"></el-progress>
                    HTML
                    <el-progress :percentage="5.9" color="#f56c6c"></el-progress> -->
                </el-card>
            </el-col>
            <el-col :span="16">
                <el-row :gutter="20" class="mgb20">
                    <el-col :span="8">
                        <el-card shadow="hover" :body-style="{padding: '0px'}">
                            <div class="grid-content grid-con-1">
                                <i class="el-icon-lx-people grid-con-icon"></i>
                                <div class="grid-cont-right">
                                    <div class="grid-num">{{userNum}}</div>
                                    <div>用户数量</div>
                                </div>
                            </div>
                        </el-card>
                    </el-col>
                    <el-col :span="8">
                        <el-card shadow="hover" :body-style="{padding: '0px'}">
                            <div class="grid-content grid-con-2">
                                <i class="el-icon-lx-notice grid-con-icon"></i>
                                <div class="grid-cont-right">
                                    <div class="grid-num">{{tackingNum}}</div>
                                    <div>正在进行的投票数量</div>
                                </div>
                            </div>
                        </el-card>
                    </el-col>
                    <el-col :span="8">
                        <el-card shadow="hover" :body-style="{padding: '0px'}">
                            <div class="grid-content grid-con-3">
                                <i class="el-icon-lx-goods grid-con-icon"></i>
                                <div class="grid-cont-right">
                                    <div class="grid-num">{{voteNum}}</div>
                                    <div>投票任务总数量</div>
                                </div>
                            </div>
                        </el-card>
                    </el-col>
                </el-row>
                <el-card shadow="hover" style="height:403px;">
                    <div slot="header" class="clearfix">
                        <span>最新注册的用户</span>
                    </div>
                        <el-table
                                :data="users"
                                border
                                class="table"
                                ref="multipleTable"
                                header-cell-class-name="table-header"
                        >
                            <el-table-column prop="username" label="用户名" align="center" min-width="4"></el-table-column>
                            <el-table-column prop="name" label="姓名" align="center" min-width="4"></el-table-column>
                            <el-table-column prop="sex" label="性别" align="center" min-width="2"></el-table-column>
                            <el-table-column prop="age" label="年龄" align="center" min-width="2"></el-table-column>
                            <el-table-column prop="address" label="地址" align="center" min-width="5"></el-table-column>
                            <el-table-column prop="phone" label="手机" align="center" min-width="4"></el-table-column>
                            <el-table-column prop="email" label="邮箱" align="center" min-width="4"></el-table-column>
                            <el-table-column prop="createTime" :formatter="dateFormat" label="创建时间" align="center" min-width="3"></el-table-column>
                        </el-table>
                </el-card>
            </el-col>
        </el-row>
    </div>
</template>

<script>
import moment from 'moment'
    export default {
        name: 'dashboard',
        data() {
            return {
                userNum: 0,
                tackingNum: 0,
                voteNum: 0,
                users: [],
                votes: [],
                page:{
                    currentPage: 1,
                    pageSize: 4
                },
            };
        },
        created() {
            this.getData();
        },
        methods: {
            dateFormat(row,column){
                var date = row[column.property];
                if(date === undefined){
                    return ''
                }
                return moment(date).format("YYYY-MM-DD HH:mm:ss")
            },
            //模拟数据
            getData() {
                //获取用户总数
                this.$axios.get('/user/num')
                    .then(response => {
                        this.userNum = response.data;
                    });
                //获取正在进行的投票数量
                this.$axios.get('/vote/tackingNum')
                    .then(response => {
                        this.tackingNum = response.data;
                    });
                //获取投票任务总数量
                this.$axios.get('/vote/num')
                    .then(response => {
                        this.voteNum = response.data;
                    });
                this.$axios.post('vote/findAll', this.page)
                    .then(response => {
                        this.votes = response.data.data;
                    })
                    .catch(() => {
                        this.$message.error('用户列表加载失败');
                    });
                this.$axios.post('user/findAll', this.page)
                    .then(response => {
                        this.users = response.data;
                    })
                    .catch(() => {
                        this.$message.error('用户列表加载失败');
                    });
                
            },
            // 编辑操作
            handleEdit(index, row) {
                this.idx = index;
                this.form = row;
                this.editVisible = true;
            },
            getSchedule(row){
                console.log(row);
                let ans;
                let date = new Date().getTime();
                let startTime = new Date(row.startTime).getTime();
                let endTime = new Date(row.endTime).getTime();
                console.log(date)
                if(date<=startTime) ans = 0;
                else if(date>=endTime) ans = 100;
                else ans = Number(date-startTime)*100/Number(endTime-startTime);
                console.log(ans);
                if(ans == 100) return 100;
                return ans.toFixed(1);
            }
        }
    };
</script>


<style scoped>
    .el-row {
        margin-bottom: 20px;
    }

    .grid-content {
        display: flex;
        align-items: center;
        height: 100px;
    }

    .grid-cont-right {
        flex: 1;
        text-align: center;
        font-size: 14px;
        color: #999;
    }

    .grid-num {
        font-size: 30px;
        font-weight: bold;
    }

    .grid-con-icon {
        font-size: 50px;
        width: 100px;
        height: 100px;
        text-align: center;
        line-height: 100px;
        color: #fff;
    }

    .grid-con-1 .grid-con-icon {
        background: rgb(45, 140, 240);
    }

    .grid-con-1 .grid-num {
        color: rgb(45, 140, 240);
    }

    .grid-con-2 .grid-con-icon {
        background: rgb(100, 213, 114);
    }

    .grid-con-2 .grid-num {
        color: rgb(45, 140, 240);
    }

    .grid-con-3 .grid-con-icon {
        background: rgb(242, 94, 67);
    }

    .grid-con-3 .grid-num {
        color: rgb(242, 94, 67);
    }

    .user-info {
        display: flex;
        align-items: center;
        padding-bottom: 20px;
        border-bottom: 2px solid #ccc;
        margin-bottom: 20px;
    }

    .user-avator {
        width: 120px;
        height: 120px;
        border-radius: 50%;
    }

    .user-info-cont {
        padding-left: 50px;
        flex: 1;
        font-size: 14px;
        color: #999;
    }

    .user-info-cont div:first-child {
        font-size: 30px;
        color: #222;
    }

    .user-info-list {
        font-size: 14px;
        color: #999;
        line-height: 25px;
    }

    .user-info-list span {
        margin-left: 70px;
    }

    .mgb20 {
        margin-bottom: 20px;
    }

    .todo-item {
        font-size: 14px;
    }

    .todo-item-del {
        text-decoration: line-through;
        color: #999;
    }

    .schart {
        width: 100%;
        height: 300px;
    }
</style>
