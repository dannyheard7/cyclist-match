import { createStyles, makeStyles, Theme } from "@material-ui/core/styles";

const useAppStyles = makeStyles((theme: Theme) => createStyles({
  toolbar: theme.mixins.toolbar,
  main: {
    maxWidth: theme.breakpoints.width('md'),
    margin: '1rem auto',
    [theme.breakpoints.down('md')]: {
      padding: theme.spacing(3)
    },
  }
}));
export default useAppStyles;