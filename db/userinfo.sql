/*
 Navicat Premium Data Transfer

 Source Server         : localhost_3306
 Source Server Type    : MySQL
 Source Server Version : 50742
 Source Host           : localhost:3306
 Source Schema         : schooluserinfo_db

 Target Server Type    : MySQL
 Target Server Version : 50742
 File Encoding         : 65001

 Date: 10/07/2023 18:32:04
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for userinfo
-- ----------------------------
DROP TABLE IF EXISTS `userinfo`;
CREATE TABLE `userinfo`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '学生编号',
  `UserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '学生姓名',
  `Sex` varchar(14) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '学生性别',
  `Phone` varchar(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '学生联系电话',
  `Description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '个人描述',
  `Hobby` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '个人爱好',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 10 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '学生信息表' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of userinfo
-- ----------------------------
INSERT INTO `userinfo` VALUES (1, '刘德华', '男', '88888888', '大帅哥一个！', '唱歌');
INSERT INTO `userinfo` VALUES (5, '追逐时光者', '男', '168888', '一个帅气小伙', '写博客，分享文章');
INSERT INTO `userinfo` VALUES (6, '小美丽', '男', '888888', '看书', '学习');
INSERT INTO `userinfo` VALUES (8, 'hajimi', 'male', '10086', 'miao', '唱跳');
INSERT INTO `userinfo` VALUES (9, 'little cat', 'female', '131313', 'miao miao~', 'pingpong');

SET FOREIGN_KEY_CHECKS = 1;
