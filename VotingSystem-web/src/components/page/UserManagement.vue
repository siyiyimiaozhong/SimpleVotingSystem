<template>
    <div>
        <div class="crumbs">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item>
                    <i class="el-icon-lx-cascades"></i> 用户管理
                </el-breadcrumb-item>
            </el-breadcrumb>
        </div>
        <div class="container">
            <div class="handle-box">
                <el-button
                        type="primary"
                        icon="el-icon-delete"
                        class="handle-del mr10"
                        @click="delAllSelection"
                >批量删除
                </el-button>
                <el-input v-model="page.search" placeholder="姓名" class="handle-input mr10"></el-input>
                <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
            </div>
            <el-table
                    :data="users"
                    border
                    class="table"
                    ref="multipleTable"
                    header-cell-class-name="table-header"
                    @selection-change="handleSelectionChange"
            >
                <el-table-column type="selection" width="55" align="center"></el-table-column>
                <el-table-column prop="id" label="序号" width="55" align="center" min-width="1">
                    <template slot-scope="scope">
                        {{scope.$index+1+(page.currentPage-1)*page.pageSize}}
                    </template>
                </el-table-column>
                <el-table-column prop="username" label="用户名" align="center" min-width="3"></el-table-column>
                <el-table-column prop="name" label="姓名" align="center" min-width="3"></el-table-column>
                <el-table-column prop="sex" label="性别" align="center" min-width="1"></el-table-column>
                <el-table-column prop="age" label="年龄" align="center" min-width="1"></el-table-column>
                <el-table-column prop="address" label="地址" align="center" min-width="4"></el-table-column>
                <el-table-column prop="phone" label="手机" align="center" min-width="3"></el-table-column>
                <el-table-column prop="email" label="邮箱" align="center" min-width="3"></el-table-column>
                <el-table-column prop="createTime" :formatter="dateFormat" label="创建时间" align="center" min-width="4"></el-table-column>
                <el-table-column label="操作" width="180" align="center" min-width="3">
                    <template slot-scope="scope">
                        <el-button
                                type="text"
                                icon="el-icon-edit"
                                @click="handleEdit(scope.$index, scope.row)"
                        >编辑
                        </el-button>
                        <el-button
                                type="text"
                                icon="el-icon-delete"
                                class="red"
                                @click="handleDelete(scope.$index, scope.row)"
                        >删除
                        </el-button>
                    </template>
                </el-table-column>
            </el-table>
            <div class="pagination">
                <el-pagination
                        background
                        layout="total, prev, pager, next, jumper"
                        current-page="page.currentPage"
                        page-size="page.pageSize"
                        :total="page.pageTotal"
                        @current-change="handlePageChange"
                ></el-pagination>
            </div>
        </div>

        <!-- 编辑弹出框 -->
        <el-dialog title="编辑" :visible.sync="editVisible" width="30%">
            <el-form ref="form" :model="form" label-width="70px" :rules="rules" style="width: 90%;">
                <el-form-item label="姓名" prop="name">
                    <el-input v-model="form.name"></el-input>
                </el-form-item>
                <el-form-item label="性别">
                    <el-select v-model="form.sex" placeholder="请选择性别">
                        <el-option label="男" value="男"></el-option>
                        <el-option label="女" value="女"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="年龄" prop="age">
                    <el-input v-model="form.age"></el-input>
                </el-form-item>
                <el-form-item label="地址" prop="address">
                    <el-input v-model="form.address"></el-input>
                </el-form-item>
                <el-form-item label="手机" prop="phone">
                    <el-input v-model="form.phone"></el-input>
                </el-form-item>
                <el-form-item label="邮箱" prop="email">
                    <el-input v-model="form.email"></el-input>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="editVisible = false">取 消</el-button>
                <el-button type="primary" @click="update">修 改</el-button>
            </span>
        </el-dialog>
    </div>
</template>

<script>
    import moment from 'moment'
    export default {
        name: 'basetable',
        data() {
            return {
                page:{
                    currentPage: 1,
                    pageSize: 10,
                    pageTotal: 0,
                    search: ''
                },
                users: [],
                multipleSelection: [],
                delStr: '',
                editVisible: false,
                form: {},
                idx: -1,
                id: -1,
                rules: {
                    name:[
                        { required: true, message: '请输入姓名', trigger: 'blur' },
                        { pattern: /^[\u4E00-\u9FA5]+$/, message: '姓名只能包含中文'}
                    ],
                    age:[
                        {required: true, message: '请输入您的年龄', trigger: 'blur'},
                        { pattern: /^(?:[1-9][0-9]?|1[01][0-9]|120)$/, message: '年龄格式不合法'}
                    ],
                    address:[
                        {required: true, message: '请输入您的地址', trigger: 'blur'}
                    ],
                    phone:[
                        {required: true, message: '请输入您的手机号码', trigger: 'blur'},
                        { pattern: /^1(3|4|5|6|7|8|9)\d{9}$/, message: '手机号码格式不合法'}
                    ],
                    email:[
                        {required: true, message: '请输入您的邮箱地址', trigger: 'blur'},
                        { pattern: /^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/, message: '邮箱格式格式不合法'}
                    ],

                }
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
            // 获取 easy-mock 的模拟数据
            getData() {
                //获取用户总数
                this.$axios.get('/user/num')
                    .then(response => {
                        this.page.pageTotal = response.data;
                    });
                //获取用户信息
                this.$axios.post('user/findAll', this.page)
                    .then(response => {
                        this.users = response.data;
                    })
                    .catch(() => {
                        this.$message.error('用户列表加载失败');
                    });
            },
            //翻页操作
            handlePageChange(currentPage){
                this.page.currentPage = currentPage;
                this.$axios.post('user/findAll', this.page)
                    .then(response => {
                        this.users = response.data;
                    })
                    .catch(() => {
                        this.$message.error('用户列表加载失败');
                    });
            },
            // 触发搜索按钮
            handleSearch() {
                this.handlePageChange(1);
            },
            // 删除操作
            handleDelete(index, row) {
                // 二次确认删除
                this.$confirm('确定要删除吗？', '提示', {
                        type: 'warning'
                    })
                    .then(() => {
                        this.$axios
                            .post('/user/delete',{id: row.id})
                            .then(response => {
                                if(response.data.status == 200){
                                    this.$message.success("删除成功");
                                }else{
                                    this.$message.error("删除失败");
                                }
                            })
                            .catch(()=>{
                                this.$message.error("删除失败");
                            });
                        this.users.splice(index, 1);
                    })
                    .catch(() => {
                    });
            },
            // 多选操作
            handleSelectionChange(val) {
                this.multipleSelection = val;
            },
            delAllSelection() {
                this.delStr = '(';
                const length = this.multipleSelection.length;
                let str = '';
                for (let i = 0; i < length; i++) {
                    str += this.multipleSelection[i].name + ' ';
                    if(i != 0) this.delStr+=',';
                    this.delStr+=this.multipleSelection[i].id;
                }
                this.delStr+=')';
                this.$axios.post('/user/deleteUsers',this.multipleSelection)
                    .then(response => {
                        if(response.data.status == 200){
                            this.$message.success(`删除 ${str} 成功`)
                        }
                    })
                    .catch(() => this.$message.error("删除失败"));
                this.multipleSelection = [];
                this.getData();
            },
            // 编辑操作
            handleEdit(index, row) {
                this.idx = index;
                this.form = row;
                this.editVisible = true;
            },
            // 保存编辑
            update() {
                this.form.age = Number(this.form.age);
                this.$axios
                    .post('/user/update',this.form)
                    .then(response => {
                        var data = response.data;
                        if(data.status == 200){
                            this.$message.success("用户信息更新成功");
                        }else{
                            this.$message.error("用户信息更新失败");
                        }
                    })
                    .catch(()=>this.$message.error("用户信息更新失败"));
                this.editVisible = false;
                this.$set(this.users, this.idx, this.form);
            }
        }
    };
</script>

<style scoped>
    .handle-box {
        margin-bottom: 20px;
    }

    .handle-select {
        width: 120px;
    }

    .handle-input {
        width: 300px;
        display: inline-block;
    }

    .table {
        width: 100%;
        font-size: 14px;
    }

    .red {
        color: #ff0000;
    }

    .mr10 {
        margin-right: 10px;
    }

    .table-td-thumb {
        display: block;
        margin: auto;
        width: 40px;
        height: 40px;
    }
</style>
